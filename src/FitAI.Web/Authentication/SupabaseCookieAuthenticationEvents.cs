using System.Collections.Concurrent;
using System.Globalization;
using FitAI.Application.Features.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FitAI.Web.Authentication;

public sealed class SupabaseCookieAuthenticationEvents(
    ISupabaseAuthService authService,
    ILogger<SupabaseCookieAuthenticationEvents> logger)
    : CookieAuthenticationEvents
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim>
        RefreshLocks = new();

    private static readonly TimeSpan RefreshBeforeExpiry =
        TimeSpan.FromHours(2);

    public override async Task ValidatePrincipal(
        CookieValidatePrincipalContext context)
    {
        var userId = context.Principal?
            .FindFirst(AuthConstants.SupabaseUserIdClaim)?
            .Value;

        userId ??= context.Principal?
            .FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?
            .Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            await RejectPrincipalAsync(
                context,
                "Authentication cookie does not contain a user ID.");

            return;
        }

        var expiresAtText = context.Properties.GetTokenValue(
            AuthConstants.TokenExpiresAtName);

        if (!TryParseExpiresAt(
                expiresAtText,
                out var expiresAtUtc))
        {
            await RejectPrincipalAsync(
                context,
                "Authentication cookie does not contain a valid token expiry.");

            return;
        }

        /*
         * Token ยังไม่ใกล้หมดอายุ จึงไม่ต้อง Refresh
         */
        if (expiresAtUtc - DateTimeOffset.UtcNow
            > RefreshBeforeExpiry)
        {
            return;
        }

        var refreshLock = RefreshLocks.GetOrAdd(
            userId,
            _ => new SemaphoreSlim(1, 1));

        await refreshLock.WaitAsync(
            context.HttpContext.RequestAborted);

        try
        {
            /*
             * อ่านค่าอีกครั้ง เพราะ Request อื่นอาจ Refresh ไปแล้ว
             * ระหว่างที่ Request นี้รอ Lock
             */
            expiresAtText = context.Properties.GetTokenValue(
                AuthConstants.TokenExpiresAtName);

            if (TryParseExpiresAt(
                    expiresAtText,
                    out expiresAtUtc)
                && expiresAtUtc - DateTimeOffset.UtcNow
                    > RefreshBeforeExpiry)
            {
                return;
            }

            var refreshToken = context.Properties.GetTokenValue(
                AuthConstants.RefreshTokenName);

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                await RejectPrincipalAsync(
                    context,
                    "Refresh token is missing.");

                return;
            }

            var newSession =
                await authService.RefreshSessionAsync(
                    new RefreshSessionCommand
                    {
                        RefreshToken = refreshToken
                    },
                    context.HttpContext.RequestAborted);

            UpdateAuthenticationTokens(
                context.Properties,
                newSession);

            /*
             * บอก Cookie Middleware ให้ออก Cookie ใหม่
             */
            context.ShouldRenew = true;

            logger.LogInformation(
                "Supabase session refreshed for user {UserId}.",
                userId);
        }
        catch (AuthServiceException ex)
        {
            logger.LogWarning(
                ex,
                "Supabase refresh token was rejected for user {UserId}.",
                userId);

            await RejectPrincipalAsync(
                context,
                "Supabase session could not be refreshed.");
        }
        catch (OperationCanceledException)
            when (context.HttpContext.RequestAborted
                .IsCancellationRequested)
        {
            // Request ถูกยกเลิก ไม่ต้องเปลี่ยนสถานะ Login
        }
        catch (Exception ex)
        {
            /*
             * Network ล่มชั่วคราวไม่ควร Logout ทันที
             * หาก access token ยังไม่หมดอายุ
             */
            logger.LogError(
                ex,
                "Unexpected error while refreshing Supabase session for user {UserId}.",
                userId);

            if (expiresAtUtc <= DateTimeOffset.UtcNow)
            {
                await RejectPrincipalAsync(
                    context,
                    "Authentication session has expired.");
            }
        }
        finally
        {
            refreshLock.Release();
        }
    }
    public override Task RedirectToLogin(
    RedirectContext<CookieAuthenticationOptions> context)
    {
        if (context.Request.Path
            .StartsWithSegments("/account"))
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            return Task.CompletedTask;
        }

        context.Response.Redirect(
            context.RedirectUri);

        return Task.CompletedTask;
    }

    private static void UpdateAuthenticationTokens(
        AuthenticationProperties properties,
        AuthSessionDto session)
    {
        properties.StoreTokens(
        [
            new AuthenticationToken
            {
                Name = AuthConstants.AccessTokenName,
                Value = session.AccessToken
            },
            new AuthenticationToken
            {
                Name = AuthConstants.RefreshTokenName,
                Value = session.RefreshToken
            },
            new AuthenticationToken
            {
                Name = AuthConstants.TokenExpiresAtName,
                Value = session.ExpiresAtUtc.ToString(
                    "O",
                    CultureInfo.InvariantCulture)
            }
        ]);

        /*
         * Cookie อายุ 7 วัน แต่ Supabase token จะถูก Refresh
         * ภายใน Cookie ตามเวลาหมดอายุจริงของ token
         */
        properties.IssuedUtc = DateTimeOffset.UtcNow;
        properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7);
        properties.IsPersistent = true;
        properties.AllowRefresh = true;
    }

    private static bool TryParseExpiresAt(
        string? value,
        out DateTimeOffset expiresAtUtc)
    {
        return DateTimeOffset.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal
            | DateTimeStyles.AdjustToUniversal,
            out expiresAtUtc);
    }

    private static async Task RejectPrincipalAsync(
        CookieValidatePrincipalContext context,
        string reason)
    {
        context.RejectPrincipal();

        await context.HttpContext.SignOutAsync(
            AuthConstants.CookieScheme);

        var logger = context.HttpContext.RequestServices
            .GetRequiredService<
                ILogger<SupabaseCookieAuthenticationEvents>>();

        logger.LogWarning(
            "Authentication principal rejected: {Reason}",
            reason);
    }
}
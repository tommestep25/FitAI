using System.Globalization;
using System.Security.Claims;
using FitAI.Application.Features.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FitAI.Web.Authentication;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/account/login",
            LoginAsync);

        endpoints.MapPost(
            "/account/logout",
            LogoutAsync);

        endpoints.MapGet(
                "/account/session",
                GetSession)
            .RequireAuthorization();

        return endpoints;
    }

    private static async Task<IResult> LoginAsync(
    HttpContext httpContext,
    ISupabaseAuthService authService,
    [FromForm] LoginRequest request,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.Redirect(
                "/login?error=missing_credentials");
        }

        try
        {
            var session = await authService.LoginAsync(
                new LoginCommand
                {
                    Email = request.Email.Trim(),
                    Password = request.Password
                },
                cancellationToken);

            var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                session.UserId.ToString()),

            new(
                ClaimTypes.Name,
                session.Email),

            new(
                ClaimTypes.Email,
                session.Email),

            new(
                AuthConstants.SupabaseUserIdClaim,
                session.UserId.ToString())
        };

            var identity = new ClaimsIdentity(
                claims,
                AuthConstants.CookieScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

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
        Value = session.ExpiresAtUtc.ToString("O")
    }
]);

            await httpContext.SignInAsync(
                AuthConstants.CookieScheme,
                principal,
                properties);

            var destination =
                IsLocalReturnUrl(request.ReturnUrl)
                    ? request.ReturnUrl!
                    : "/";

            var continueUrl =
                $"/account/continue?returnUrl=" +
                Uri.EscapeDataString(destination);

            return Results.Redirect(continueUrl);

        }
        catch (AuthServiceException)
        {
            return Results.Redirect(
                "/login?error=invalid_credentials");
        }
    }

    private static async Task<IResult> LogoutAsync(
        HttpContext httpContext,
        ISupabaseAuthService authService,
        CancellationToken cancellationToken)
    {
        var accessToken =
            await httpContext.GetTokenAsync(
                AuthConstants.CookieScheme,
                AuthConstants.AccessTokenName);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            try
            {
                await authService.LogoutAsync(
                    accessToken,
                    cancellationToken);
            }
            catch
            {
                // Logout ในเครื่องต้องดำเนินต่อ
                // แม้ Supabase จะตอบกลับไม่สำเร็จ
            }
        }

        await httpContext.SignOutAsync(
            AuthConstants.CookieScheme);

        return Results.Redirect("/login");
    }

    private static bool IsLocalReturnUrl(
        string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl)
               && returnUrl.StartsWith('/')
               && !returnUrl.StartsWith("//")
               && !returnUrl.StartsWith(@"/\");
    }
    private static IResult GetSession(
    HttpContext httpContext)
    {
        return Results.Json(new
        {
            isAuthenticated =
                httpContext.User.Identity?.IsAuthenticated == true,

            userId = httpContext.User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value,

            email = httpContext.User.FindFirst(
                ClaimTypes.Email)?.Value
        });
    }
}
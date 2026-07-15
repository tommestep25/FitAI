using System.Net.Http.Json;
using System.Text.Json;
using FitAI.Application.Features.Authentication;
using Microsoft.Extensions.Options;

namespace FitAI.Infrastructure.Authentication;

public sealed class SupabaseAuthService(
    HttpClient httpClient,
    IOptions<SupabaseAuthOptions> options)
    : ISupabaseAuthService
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    private readonly SupabaseAuthOptions _options = options.Value;

    public async Task<AuthSessionDto> LoginAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        ValidateEmailAndPassword(
            command.Email,
            command.Password);

        using var request = CreateRequest(
            HttpMethod.Post,
            "auth/v1/token?grant_type=password");

        request.Content = JsonContent.Create(new
        {
            email = command.Email.Trim(),
            password = command.Password
        });

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        var authResponse =
            await ReadResponseAsync<SupabaseAuthResponse>(
                response,
                cancellationToken);

        return MapSession(authResponse);
    }

    public async Task<RegisterResultDto> RegisterAsync(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        ValidateEmailAndPassword(
            command.Email,
            command.Password);

        if (string.IsNullOrWhiteSpace(command.FirstName))
        {
            throw new ArgumentException(
                "First name is required.",
                nameof(command));
        }

        using var request = CreateRequest(
            HttpMethod.Post,
            "auth/v1/signup");

        request.Content = JsonContent.Create(new
        {
            email = command.Email.Trim(),
            password = command.Password,

            data = new
            {
                display_name = command.DisplayName,
                first_name = command.FirstName.Trim(),
                last_name = command.LastName.Trim()
            },

            gotrue_meta_security = new
            {
                captcha_token = (string?)null
            }
        });

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        var authResponse =
            await ReadResponseAsync<SupabaseAuthResponse>(
                response,
                cancellationToken);

        var hasSession =
            !string.IsNullOrWhiteSpace(authResponse.AccessToken)
            && !string.IsNullOrWhiteSpace(
                authResponse.RefreshToken);

        return new RegisterResultDto
        {
            UserId = authResponse.User?.Id
                ?? throw new AuthServiceException(
                    "Supabase did not return a user ID."),

            Email = authResponse.User?.Email
                ?? command.Email.Trim(),

            RequiresEmailConfirmation = !hasSession,

            Session = hasSession
                ? MapSession(authResponse)
                : null
        };
    }

    public async Task<AuthSessionDto> RefreshSessionAsync(
        RefreshSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            throw new ArgumentException(
                "Refresh token is required.",
                nameof(command));
        }

        using var request = CreateRequest(
            HttpMethod.Post,
            "auth/v1/token?grant_type=refresh_token");

        request.Content = JsonContent.Create(new
        {
            refresh_token = command.RefreshToken
        });

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        var authResponse =
            await ReadResponseAsync<SupabaseAuthResponse>(
                response,
                cancellationToken);

        return MapSession(authResponse);
    }
    public async Task ResendConfirmationAsync(
    ResendConfirmationCommand command,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email))
        {
            throw new ArgumentException(
                "Email is required.",
                nameof(command));
        }

        var resendUrl = "auth/v1/resend";

        if (!string.IsNullOrWhiteSpace(
                _options.EmailConfirmationRedirectUrl))
        {
            resendUrl +=
                $"?redirect_to={Uri.EscapeDataString(
                    _options.EmailConfirmationRedirectUrl)}";
        }

        using var request = CreateRequest(
            HttpMethod.Post,
            resendUrl);

        request.Content = JsonContent.Create(new
        {
            type = "signup",
            email = command.Email.Trim()
        });

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowAuthExceptionAsync(
                response,
                cancellationToken);
        }
    }

    public async Task LogoutAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return;
        }

        using var request = CreateRequest(
            HttpMethod.Post,
            "auth/v1/logout");

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                accessToken);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowAuthExceptionAsync(
                response,
                cancellationToken);
        }
    }

    private HttpRequestMessage CreateRequest(
        HttpMethod method,
        string relativeUrl)
    {
        EnsureOptionsAreValid();

        var baseUrl = _options.Url.TrimEnd('/');

        var request = new HttpRequestMessage(
            method,
            $"{baseUrl}/{relativeUrl}");

        request.Headers.TryAddWithoutValidation(
            "apikey",
            _options.AnonKey);

        request.Headers.Accept.ParseAdd(
            "application/json");

        return request;
    }

    private static async Task<T> ReadResponseAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            await ThrowAuthExceptionAsync(
                response,
                cancellationToken);
        }

        var result = await response.Content
            .ReadFromJsonAsync<T>(
                JsonOptions,
                cancellationToken);

        return result
            ?? throw new AuthServiceException(
                "Supabase returned an empty response.");
    }

    private static async Task ThrowAuthExceptionAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        SupabaseAuthErrorResponse? error = null;

        try
        {
            error = await response.Content
                .ReadFromJsonAsync<SupabaseAuthErrorResponse>(
                    JsonOptions,
                    cancellationToken);
        }
        catch (JsonException)
        {
            // ใช้ข้อความสำรองด้านล่าง
        }

        var message =
            error?.Message
            ?? error?.AlternativeMessage
            ?? error?.ErrorDescription
            ?? error?.Error
            ?? $"Supabase Auth request failed with HTTP {(int)response.StatusCode}.";

        throw new AuthServiceException(
            message,
            (int)response.StatusCode,
            error?.ErrorCode);
    }

    private static AuthSessionDto MapSession(
        SupabaseAuthResponse response)
    {
        if (response.User is null)
        {
            throw new AuthServiceException(
                "Supabase did not return user information.");
        }

        if (string.IsNullOrWhiteSpace(
                response.AccessToken)
            || string.IsNullOrWhiteSpace(
                response.RefreshToken))
        {
            throw new AuthServiceException(
                "Supabase did not return a valid session.");
        }

        var expiresAtUtc =
            response.ExpiresAtUnix.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(
                    response.ExpiresAtUnix.Value)
                : DateTimeOffset.UtcNow.AddSeconds(
                    response.ExpiresIn);

        return new AuthSessionDto
        {
            UserId = response.User.Id,
            Email = response.User.Email ?? string.Empty,
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            TokenType = response.TokenType ?? "bearer",
            ExpiresInSeconds = response.ExpiresIn,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    private void EnsureOptionsAreValid()
    {
        if (string.IsNullOrWhiteSpace(_options.Url))
        {
            throw new InvalidOperationException(
                "Supabase URL is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _options.AnonKey))
        {
            throw new InvalidOperationException(
                "Supabase anon key is not configured.");
        }
    }

    private static void ValidateEmailAndPassword(
        string email,
        string password)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "Password is required.");
        }

        if (password.Length < 6)
        {
            throw new ArgumentException(
                "Password must contain at least 6 characters.");
        }
    }
}
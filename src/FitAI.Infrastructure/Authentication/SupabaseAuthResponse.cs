using System.Text.Json.Serialization;

namespace FitAI.Infrastructure.Authentication;

internal sealed class SupabaseAuthResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; init; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("expires_at")]
    public long? ExpiresAtUnix { get; init; }

    [JsonPropertyName("user")]
    public SupabaseAuthUserResponse? User { get; init; }
}

internal sealed class SupabaseAuthUserResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }
}

internal sealed class SupabaseAuthErrorResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("error_code")]
    public string? ErrorCode { get; init; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; init; }

    [JsonPropertyName("msg")]
    public string? Message { get; init; }

    [JsonPropertyName("message")]
    public string? AlternativeMessage { get; init; }
}
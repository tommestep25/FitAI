namespace FitAI.Application.Features.Authentication;

public sealed class AuthSessionDto
{
    public Guid UserId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public string TokenType { get; init; } = "bearer";

    public int ExpiresInSeconds { get; init; }

    public DateTimeOffset ExpiresAtUtc { get; init; }
}
namespace FitAI.Application.Features.Authentication;

public sealed class RegisterResultDto
{
    public Guid UserId { get; init; }

    public string Email { get; init; } = string.Empty;

    public bool RequiresEmailConfirmation { get; init; }

    public AuthSessionDto? Session { get; init; }
}
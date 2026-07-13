namespace FitAI.Application.Features.Authentication;

public sealed class RegisterCommand
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string DisplayName =>
        $"{FirstName} {LastName}".Trim();
}
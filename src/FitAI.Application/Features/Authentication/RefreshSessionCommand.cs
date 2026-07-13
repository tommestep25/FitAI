namespace FitAI.Application.Features.Authentication;

public sealed class RefreshSessionCommand
{
    public string RefreshToken { get; init; } = string.Empty;
}
namespace FitAI.Application.Features.Authentication;

public interface ISupabaseAuthService
{
    Task<AuthSessionDto> LoginAsync(
            LoginCommand command,
            CancellationToken cancellationToken = default);

    Task<RegisterResultDto> RegisterAsync(
        RegisterCommand command,
        CancellationToken cancellationToken = default);

    Task ResendConfirmationAsync(
        ResendConfirmationCommand command,
        CancellationToken cancellationToken = default);

    Task<AuthSessionDto> RefreshSessionAsync(
        RefreshSessionCommand command,
        CancellationToken cancellationToken = default);

    Task LogoutAsync(
        string accessToken,
        CancellationToken cancellationToken = default);
}
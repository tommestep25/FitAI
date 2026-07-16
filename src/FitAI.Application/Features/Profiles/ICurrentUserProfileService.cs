namespace FitAI.Application.Features.Profiles;

public interface ICurrentUserProfileService
{
    Task<CurrentUserProfileDto?> GetCurrentAsync(
        CancellationToken cancellationToken = default);

    Task<bool> IsOnboardingCompletedAsync(
        CancellationToken cancellationToken = default);

    Task<CompleteOnboardingResult> CompleteOnboardingAsync(
        CompleteOnboardingCommand command,
        CancellationToken cancellationToken = default);
}
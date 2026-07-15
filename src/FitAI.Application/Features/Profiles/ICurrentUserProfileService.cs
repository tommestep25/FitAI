namespace FitAI.Application.Features.Profiles;

public interface ICurrentUserProfileService
{
    Task<CurrentUserProfileDto?> GetCurrentAsync(
        CancellationToken cancellationToken = default);

    Task<bool> IsOnboardingCompletedAsync(
        CancellationToken cancellationToken = default);
}
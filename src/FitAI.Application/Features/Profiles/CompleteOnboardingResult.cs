namespace FitAI.Application.Features.Profiles;

public sealed class CompleteOnboardingResult
{
    public Guid UserId { get; init; }

    public bool OnboardingCompleted { get; init; }

    public DateTimeOffset UpdatedAt { get; init; }
}
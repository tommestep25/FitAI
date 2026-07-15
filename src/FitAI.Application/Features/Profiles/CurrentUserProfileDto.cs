namespace FitAI.Application.Features.Profiles;

public sealed class CurrentUserProfileDto
{
    public Guid UserId { get; init; }

    public string? Email { get; init; }

    public string? DisplayName { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public DateOnly? BirthDate { get; init; }

    public string? Gender { get; init; }

    public decimal? HeightCm { get; init; }

    public decimal? CurrentWeightKg { get; init; }

    public decimal? GoalWeightKg { get; init; }

    public string? ActivityLevel { get; init; }

    public string? FitnessGoal { get; init; }

    public string Timezone { get; init; } = "Asia/Bangkok";

    public bool IsActive { get; init; }

    public bool OnboardingCompleted { get; init; }
}
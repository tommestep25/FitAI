namespace FitAI.Application.Features.Profiles;

public sealed class CompleteOnboardingCommand
{
    public DateOnly BirthDate { get; init; }

    public string Gender { get; init; } = string.Empty;

    public decimal HeightCm { get; init; }

    public decimal CurrentWeightKg { get; init; }

    public decimal? GoalWeightKg { get; init; }

    public string ActivityLevel { get; init; } = string.Empty;

    public string FitnessGoal { get; init; } = string.Empty;

    public string Timezone { get; init; } = "Asia/Bangkok";
}
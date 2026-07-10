namespace FitAI.Application.Features.Dashboard;

public sealed class DashboardSummaryDto
{
    public string FirstName { get; init; } = string.Empty;

    public decimal? CurrentWeightKg { get; init; }
    public decimal? GoalWeightKg { get; init; }

    public decimal? LatestInBodyWeightKg { get; init; }
    public decimal? LatestBodyFatPercentage { get; init; }
    public decimal? LatestSkeletalMuscleMassKg { get; init; }
    public int? LatestBmr { get; init; }
    public int? LatestInBodyScore { get; init; }

    public int ProteinGoalG { get; init; } = 160;
    public int CaloriesGoal { get; init; } = 2100;
}
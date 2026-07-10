namespace FitAI.Application.Features.WorkoutSets;

public sealed class CompleteWorkoutSetCommand
{
    public Guid WorkoutSetId { get; init; }

    public decimal ActualWeightKg { get; init; }

    public int ActualReps { get; init; }

    public decimal? Rpe { get; init; }

    public int? ActualRestSeconds { get; init; }
}
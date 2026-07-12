namespace FitAI.Application.Features.WorkoutSets;

public sealed class AddWorkoutSetResult
{
    public Guid Id { get; init; }
    public Guid WorkoutSessionExerciseId { get; init; }
    public int SetNumber { get; init; }

    public int? TargetReps { get; init; }
    public int? TargetRestSeconds { get; init; }

    public string SetType { get; init; } = "Working";
}
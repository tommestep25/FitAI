namespace FitAI.Application.Features.WorkoutSets;

public sealed class CompleteWorkoutSetResult
{
    public Guid WorkoutSetId { get; init; }

    public Guid WorkoutSessionExerciseId { get; init; }

    public DateTimeOffset CompletedAt { get; init; }

    public bool IsExerciseCompleted { get; init; }
}
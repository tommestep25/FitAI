namespace FitAI.Application.Features.WorkoutSets;

public sealed class AddWorkoutSetCommand
{
    public Guid WorkoutSessionExerciseId { get; init; }
}
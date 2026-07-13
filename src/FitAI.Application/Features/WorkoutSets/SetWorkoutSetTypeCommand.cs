namespace FitAI.Application.Features.WorkoutSets;

public sealed class SetWorkoutSetTypeCommand
{
    public Guid WorkoutSetId { get; init; }
    public bool IsWarmup { get; init; }
}
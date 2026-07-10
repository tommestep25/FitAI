namespace FitAI.Application.Features.WorkoutSessions;

public sealed class CompleteWorkoutCommand
{
    public Guid WorkoutSessionId { get; init; }
}
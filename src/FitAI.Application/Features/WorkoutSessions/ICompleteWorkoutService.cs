namespace FitAI.Application.Features.WorkoutSessions;

public interface ICompleteWorkoutService
{
    Task<CompleteWorkoutResult> CompleteAsync(
        CompleteWorkoutCommand command,
        CancellationToken cancellationToken = default);
}
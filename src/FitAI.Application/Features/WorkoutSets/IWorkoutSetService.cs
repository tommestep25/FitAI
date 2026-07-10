namespace FitAI.Application.Features.WorkoutSets;

public interface IWorkoutSetService
{
    Task<CompleteWorkoutSetResult> CompleteSetAsync(
        CompleteWorkoutSetCommand command,
        CancellationToken cancellationToken = default);
}
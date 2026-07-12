namespace FitAI.Application.Features.WorkoutSets;

public interface IWorkoutSetService
{
    Task<CompleteWorkoutSetResult> CompleteSetAsync(
        CompleteWorkoutSetCommand command,
        CancellationToken cancellationToken = default);

    Task<AddWorkoutSetResult> AddSetAsync(
        AddWorkoutSetCommand command,
        CancellationToken cancellationToken = default);

    Task DeleteSetAsync(
        DeleteWorkoutSetCommand command,
        CancellationToken cancellationToken = default);

    Task SetWarmupAsync(
        SetWorkoutSetTypeCommand command,
        CancellationToken cancellationToken = default);
}
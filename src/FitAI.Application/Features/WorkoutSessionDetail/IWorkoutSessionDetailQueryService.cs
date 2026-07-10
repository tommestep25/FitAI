namespace FitAI.Application.Features.WorkoutSessionDetail;

public interface IWorkoutSessionDetailQueryService
{
    Task<WorkoutSessionDetailDto?> GetAsync(
        Guid workoutSessionId,
        CancellationToken cancellationToken = default);
}
namespace FitAI.Application.Features.WorkoutSessions;

public interface IWorkoutSessionService
{
    Task<StartWorkoutSessionResult> StartAsync(
        StartWorkoutSessionCommand command,
        CancellationToken cancellationToken = default);
}
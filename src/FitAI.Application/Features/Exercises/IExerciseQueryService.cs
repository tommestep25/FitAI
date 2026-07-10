namespace FitAI.Application.Features.Exercises;

public interface IExerciseQueryService
{
    Task<IReadOnlyList<ExerciseDto>> GetExercisesAsync(CancellationToken cancellationToken = default);
}
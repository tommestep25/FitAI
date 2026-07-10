namespace FitAI.Application.Features.WorkoutTemplates;

public interface IWorkoutTemplateQueryService
{
    Task<IReadOnlyList<WorkoutTemplateDayDto>> GetTemplateDaysAsync(
        CancellationToken cancellationToken = default);
}
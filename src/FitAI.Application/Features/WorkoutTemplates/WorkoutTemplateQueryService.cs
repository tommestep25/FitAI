using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.WorkoutTemplates;

public sealed class WorkoutTemplateQueryService(IApplicationDbContext context)
    : IWorkoutTemplateQueryService
{
    public async Task<IReadOnlyList<WorkoutTemplateDayDto>> GetTemplateDaysAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.WorkoutTemplateDays
            .AsNoTracking()
            .OrderBy(x => x.SortOrder)
            .Select(x => new WorkoutTemplateDayDto
            {
                Id = x.Id,
                Name = x.Name,
                DayType = x.DayType,
                SortOrder = x.SortOrder
            })
            .ToListAsync(cancellationToken);
    }
}
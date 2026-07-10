using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.Exercises;

public sealed class ExerciseQueryService(IApplicationDbContext context) : IExerciseQueryService
{
    public async Task<IReadOnlyList<ExerciseDto>> GetExercisesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Exercises
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new ExerciseDto
            {
                Id = x.Id,
                Name = x.Name,
                DefaultSets = x.DefaultSets,
                DefaultMinReps = x.DefaultMinReps,
                DefaultMaxReps = x.DefaultMaxReps,
                DefaultRestSeconds = x.DefaultRestSeconds,
                Tempo = x.Tempo
            })
            .ToListAsync(cancellationToken);
    }
}
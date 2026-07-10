using FitAI.Application.Common.Interfaces;
using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.WorkoutSessions;

public sealed class WorkoutSessionService(IApplicationDbContext context)
    : IWorkoutSessionService
{
    public async Task<StartWorkoutSessionResult> StartAsync(
        StartWorkoutSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("User not found.");

        var templateDay = await context.WorkoutTemplateDays
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == command.WorkoutTemplateDayId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Workout template day not found.");

        var templateExercises = await context.WorkoutTemplateExercises
            .AsNoTracking()
            .Where(x => x.WorkoutTemplateDayId == command.WorkoutTemplateDayId)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        if (templateExercises.Count == 0)
        {
            throw new InvalidOperationException(
                "Workout template has no exercises.");
        }

        var now = DateTimeOffset.UtcNow;

        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,

            // ไม่ใช่ workout.workout_days
            WorkoutDayId = null,

            SourceTemplateDayId = templateDay.Id,
            SessionDate = DateOnly.FromDateTime(now.UtcDateTime),
            StartTime = now,
            SessionName = templateDay.Name,
            CreatedAt = now
        };

        foreach (var item in templateExercises)
        {
            var sessionExercise = new WorkoutSessionExercise
            {
                Id = Guid.NewGuid(),
                ExerciseId = item.ExerciseId,
                SortOrder = item.SortOrder,

                TargetSets = item.TargetSets,
                TargetMinReps = item.TargetMinReps,
                TargetMaxReps = item.TargetMaxReps,
                TargetRestSeconds = item.TargetRestSeconds,
                TargetRpe = item.TargetRpe,

                IsCompleted = false,
                Notes = item.Notes,
                CreatedAt = now
            };

            for (var setNumber = 1; setNumber <= item.TargetSets; setNumber++)
            {
                sessionExercise.Sets.Add(new WorkoutSet
                {
                    Id = Guid.NewGuid(),
                    SetNumber = setNumber,
                    TargetReps = item.TargetMaxReps,
                    TargetRestSeconds = item.TargetRestSeconds,
                    SetType = "Working",
                    IsWarmup = false,
                    IsCompleted = false,
                    IsSkipped = false,
                    CreatedAt = now
                });
            }

            session.Exercises.Add(sessionExercise);
        }

        context.WorkoutSessions.Add(session);

        await context.SaveChangesAsync(cancellationToken);

        return new StartWorkoutSessionResult
        {
            WorkoutSessionId = session.Id
        };
    }
}
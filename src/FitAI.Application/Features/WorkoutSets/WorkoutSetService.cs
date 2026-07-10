using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.WorkoutSets;

public sealed class WorkoutSetService(IApplicationDbContext context)
    : IWorkoutSetService
{
    public async Task<CompleteWorkoutSetResult> CompleteSetAsync(
        CompleteWorkoutSetCommand command,
        CancellationToken cancellationToken = default)
    {
        ValidateCommand(command);

        var workoutSet = await context.WorkoutSets
            .FirstOrDefaultAsync(
                x => x.Id == command.WorkoutSetId,
                cancellationToken)
            ?? throw new InvalidOperationException("Workout set not found.");

        if (workoutSet.IsSkipped)
        {
            throw new InvalidOperationException(
                "Skipped set cannot be completed.");
        }

        var now = DateTimeOffset.UtcNow;

        workoutSet.ActualWeightKg = command.ActualWeightKg;
        workoutSet.ActualReps = command.ActualReps;
        workoutSet.Rpe = command.Rpe;
        workoutSet.ActualRestSeconds = command.ActualRestSeconds;

        workoutSet.IsCompleted = true;
        workoutSet.CompletedAt = now;
        workoutSet.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);

        var hasIncompleteSets = await context.WorkoutSets
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.WorkoutSessionExerciseId
                        == workoutSet.WorkoutSessionExerciseId
                    && !x.IsCompleted
                    && !x.IsSkipped,
                cancellationToken);

        var isExerciseCompleted = !hasIncompleteSets;

        if (isExerciseCompleted)
        {
            var sessionExercise =
                await context.WorkoutSessionExercises
                    .FirstOrDefaultAsync(
                        x => x.Id
                            == workoutSet.WorkoutSessionExerciseId,
                        cancellationToken)
                ?? throw new InvalidOperationException(
                    "Workout session exercise not found.");

            sessionExercise.IsCompleted = true;
            sessionExercise.CompletedAt = now;
            sessionExercise.UpdatedAt = now;

            await context.SaveChangesAsync(cancellationToken);
        }

        return new CompleteWorkoutSetResult
        {
            WorkoutSetId = workoutSet.Id,
            WorkoutSessionExerciseId =
                workoutSet.WorkoutSessionExerciseId,
            CompletedAt = now,
            IsExerciseCompleted = isExerciseCompleted
        };
    }

    private static void ValidateCommand(
        CompleteWorkoutSetCommand command)
    {
        if (command.WorkoutSetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Workout set ID is required.");
        }

        if (command.ActualWeightKg < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.ActualWeightKg),
                "Weight cannot be negative.");
        }

        if (command.ActualReps <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.ActualReps),
                "Reps must be greater than zero.");
        }

        if (command.Rpe is < 1 or > 10)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.Rpe),
                "RPE must be between 1 and 10.");
        }

        if (command.ActualRestSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.ActualRestSeconds),
                "Rest seconds cannot be negative.");
        }
    }
}
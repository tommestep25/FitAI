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
    public async Task<AddWorkoutSetResult> AddSetAsync(
    AddWorkoutSetCommand command,
    CancellationToken cancellationToken = default)
    {
        if (command.WorkoutSessionExerciseId == Guid.Empty)
        {
            throw new ArgumentException(
                "Workout session exercise ID is required.");
        }

        var sessionExercise = await context.WorkoutSessionExercises
            .Include(x => x.Sets)
            .FirstOrDefaultAsync(
                x => x.Id == command.WorkoutSessionExerciseId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Workout session exercise not found.");

        if (sessionExercise.IsCompleted)
        {
            throw new InvalidOperationException(
                "Cannot add a set to a completed exercise.");
        }

        var nextSetNumber = sessionExercise.Sets.Count == 0
            ? 1
            : sessionExercise.Sets.Max(x => x.SetNumber) + 1;

        var now = DateTimeOffset.UtcNow;

        var workoutSet = new FitAI.Domain.Entities.WorkoutSet
        {
            Id = Guid.NewGuid(),
            WorkoutSessionExerciseId = sessionExercise.Id,
            SetNumber = nextSetNumber,

            TargetReps = sessionExercise.TargetMaxReps,
            TargetRestSeconds = sessionExercise.TargetRestSeconds,

            SetType = "Working",
            IsWarmup = false,
            IsCompleted = false,
            IsSkipped = false,

            CreatedAt = now
        };

        context.WorkoutSets.Add(workoutSet);

        sessionExercise.TargetSets = nextSetNumber;
        sessionExercise.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);

        return new AddWorkoutSetResult
        {
            Id = workoutSet.Id,
            WorkoutSessionExerciseId = sessionExercise.Id,
            SetNumber = workoutSet.SetNumber,
            TargetReps = workoutSet.TargetReps,
            TargetRestSeconds = workoutSet.TargetRestSeconds,
            SetType = workoutSet.SetType
        };
    }
    public async Task DeleteSetAsync(
    DeleteWorkoutSetCommand command,
    CancellationToken cancellationToken = default)
    {
        if (command.WorkoutSetId == Guid.Empty)
        {
            throw new ArgumentException("Workout set ID is required.");
        }

        var workoutSet = await context.WorkoutSets
            .FirstOrDefaultAsync(
                x => x.Id == command.WorkoutSetId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Workout set not found.");

        if (workoutSet.IsCompleted)
        {
            throw new InvalidOperationException(
                "Completed set cannot be deleted.");
        }

        var highestSetNumber = await context.WorkoutSets
            .Where(x =>
                x.WorkoutSessionExerciseId
                    == workoutSet.WorkoutSessionExerciseId)
            .MaxAsync(x => x.SetNumber, cancellationToken);

        if (workoutSet.SetNumber != highestSetNumber)
        {
            throw new InvalidOperationException(
                "Only the last set can be deleted.");
        }

        var sessionExercise = await context.WorkoutSessionExercises
            .FirstOrDefaultAsync(
                x => x.Id == workoutSet.WorkoutSessionExerciseId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Workout session exercise not found.");

        context.WorkoutSets.Remove(workoutSet);

        sessionExercise.TargetSets = Math.Max(
            0,
            highestSetNumber - 1);

        sessionExercise.UpdatedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task SetWarmupAsync(
    SetWorkoutSetTypeCommand command,
    CancellationToken cancellationToken = default)
    {
        if (command.WorkoutSetId == Guid.Empty)
        {
            throw new ArgumentException("Workout set ID is required.");
        }

        var workoutSet = await context.WorkoutSets
            .FirstOrDefaultAsync(
                x => x.Id == command.WorkoutSetId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Workout set not found.");

        if (workoutSet.IsCompleted)
        {
            throw new InvalidOperationException(
                "Completed set type cannot be changed.");
        }

        workoutSet.IsWarmup = command.IsWarmup;
        workoutSet.SetType = command.IsWarmup
            ? "Warmup"
            : "Working";

        workoutSet.UpdatedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
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
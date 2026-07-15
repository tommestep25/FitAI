using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.WorkoutSessionDetail;

public sealed class WorkoutSessionDetailQueryService(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IWorkoutSessionDetailQueryService
{
    public async Task<WorkoutSessionDetailDto?> GetAsync(
        Guid workoutSessionId,
        CancellationToken cancellationToken = default)
    {
        var userId = await currentUserService.GetUserIdAsync(
    cancellationToken);

        var session = await context.WorkoutSessions
            .AsNoTracking()
            .Include(x => x.Exercises)
                .ThenInclude(x => x.Exercise)
            .Include(x => x.Exercises)
                .ThenInclude(x => x.Sets)
            .FirstOrDefaultAsync(
                x => x.Id == workoutSessionId
                     && x.UserId == userId,
                cancellationToken);

        if (session is null)
        {
            return null;
        }

        var exerciseIds = session.Exercises
            .Select(x => x.ExerciseId)
            .Distinct()
            .ToList();

        var previousCandidates = await context.WorkoutSessionExercises
            .AsNoTracking()
            .Where(x =>
                exerciseIds.Contains(x.ExerciseId)
                && x.WorkoutSessionId != session.Id
                && x.WorkoutSession.UserId == session.UserId
                && x.WorkoutSession.Status == "Completed"
                && (
                    !session.StartTime.HasValue
                    || x.WorkoutSession.StartTime < session.StartTime
                ))
            .Select(x => new PreviousExerciseCandidate
            {
                WorkoutSessionExerciseId = x.Id,
                ExerciseId = x.ExerciseId,
                SessionStartTime = x.WorkoutSession.StartTime
            })
            .OrderByDescending(x => x.SessionStartTime)
            .ToListAsync(cancellationToken);

        /*
         * เลือก SessionExercise ล่าสุดเพียงรายการเดียว
         * สำหรับแต่ละ Exercise
         */
        var latestPreviousExerciseByExerciseId =
            previousCandidates
                .GroupBy(x => x.ExerciseId)
                .ToDictionary(
                    group => group.Key,
                    group => group.First());

        var previousSessionExerciseIds =
            latestPreviousExerciseByExerciseId.Values
                .Select(x => x.WorkoutSessionExerciseId)
                .ToList();

        var previousSets = previousSessionExerciseIds.Count == 0
            ? []
            : await context.WorkoutSets
                .AsNoTracking()
                .Where(x =>
                    previousSessionExerciseIds.Contains(
                        x.WorkoutSessionExerciseId)
                    && x.IsCompleted
                    && !x.IsSkipped)
                .OrderBy(x => x.SetNumber)
                .Select(x => new PreviousSetData
                {
                    WorkoutSessionExerciseId =
                        x.WorkoutSessionExerciseId,
                    SetNumber = x.SetNumber,
                    ActualWeightKg = x.ActualWeightKg,
                    ActualReps = x.ActualReps,
                    Rpe = x.Rpe
                })
                .ToListAsync(cancellationToken);

        var previousSetsBySessionExerciseId =
            previousSets
                .GroupBy(x => x.WorkoutSessionExerciseId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToDictionary(
                        set => set.SetNumber));

        var exerciseDtos = session.Exercises
            .OrderBy(x => x.SortOrder)
            .Select(sessionExercise =>
            {
                previousSetsBySessionExerciseId.TryGetValue(
                    latestPreviousExerciseByExerciseId
                        .GetValueOrDefault(sessionExercise.ExerciseId)?
                        .WorkoutSessionExerciseId
                    ?? Guid.Empty,
                    out var previousSetsByNumber);

                var setDtos = sessionExercise.Sets
    .OrderBy(x => x.SetNumber)
    .Select(set =>
    {
        PreviousSetData? previousSet = null;

        if (previousSetsByNumber is not null)
        {
            previousSetsByNumber.TryGetValue(
                set.SetNumber,
                out previousSet);
        }

        return new WorkoutSetDto
        {
            Id = set.Id,
            SetNumber = set.SetNumber,

            TargetWeightKg = set.TargetWeightKg,
            ActualWeightKg = set.ActualWeightKg,

            TargetReps = set.TargetReps,
            ActualReps = set.ActualReps,

            Rpe = set.Rpe,

            TargetRestSeconds = set.TargetRestSeconds,
            ActualRestSeconds = set.ActualRestSeconds,

            SetType = set.SetType,

            IsWarmup = set.IsWarmup,
            IsCompleted = set.IsCompleted,
            IsSkipped = set.IsSkipped,

            PreviousWeightKg = previousSet?.ActualWeightKg,
            PreviousReps = previousSet?.ActualReps,
            PreviousRpe = previousSet?.Rpe
        };
    })
    .ToList();

                return new WorkoutExerciseDto
                {
                    WorkoutSessionExerciseId =
                        sessionExercise.Id,

                    ExerciseId = sessionExercise.ExerciseId,

                    ExerciseName =
                        sessionExercise.Exercise.Name,

                    SortOrder = sessionExercise.SortOrder,

                    TargetSets =
                        sessionExercise.TargetSets,

                    TargetMinReps =
                        sessionExercise.TargetMinReps,

                    TargetMaxReps =
                        sessionExercise.TargetMaxReps,

                    TargetRestSeconds =
                        sessionExercise.TargetRestSeconds,

                    TargetRpe =
                        sessionExercise.TargetRpe,

                    IsCompleted =
                        sessionExercise.IsCompleted,

                    Sets = setDtos
                };
            })
            .ToList();

        return new WorkoutSessionDetailDto
        {
            SessionId = session.Id,
            SessionName =
                session.SessionName ?? "Workout",

            SessionDate = session.SessionDate,
            StartTime = session.StartTime,
            Status = session.Status,
            Exercises = exerciseDtos
        };
    }

    private sealed class PreviousExerciseCandidate
    {
        public Guid WorkoutSessionExerciseId { get; init; }
        public Guid ExerciseId { get; init; }
        public DateTimeOffset? SessionStartTime { get; init; }
    }

    private sealed class PreviousSetData
    {
        public Guid WorkoutSessionExerciseId { get; init; }
        public int SetNumber { get; init; }

        public decimal? ActualWeightKg { get; init; }
        public int? ActualReps { get; init; }
        public decimal? Rpe { get; init; }
    }
}
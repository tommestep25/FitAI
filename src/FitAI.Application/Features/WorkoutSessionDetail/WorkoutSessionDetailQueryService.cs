using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.WorkoutSessionDetail;

public sealed class WorkoutSessionDetailQueryService(
    IApplicationDbContext context)
    : IWorkoutSessionDetailQueryService
{
    public async Task<WorkoutSessionDetailDto?> GetAsync(
        Guid workoutSessionId,
        CancellationToken cancellationToken = default)
    {
        return await context.WorkoutSessions
            .AsNoTracking()
            .Where(session => session.Id == workoutSessionId)
            .Select(session => new WorkoutSessionDetailDto
            {
                SessionId = session.Id,
                SessionName = session.SessionName ?? "Workout",
                SessionDate = session.SessionDate,
                StartTime = session.StartTime,
                Status = session.Status,
                Exercises = session.Exercises
                    .OrderBy(exercise => exercise.SortOrder)
                    .Select(exercise => new WorkoutExerciseDto
                    {
                        WorkoutSessionExerciseId = exercise.Id,
                        ExerciseId = exercise.ExerciseId,
                        ExerciseName = exercise.Exercise.Name,

                        SortOrder = exercise.SortOrder,
                        TargetSets = exercise.TargetSets,
                        TargetMinReps = exercise.TargetMinReps,
                        TargetMaxReps = exercise.TargetMaxReps,
                        TargetRestSeconds = exercise.TargetRestSeconds,
                        TargetRpe = exercise.TargetRpe,

                        IsCompleted = exercise.IsCompleted,

                        Sets = exercise.Sets
                            .OrderBy(set => set.SetNumber)
                            .Select(set => new WorkoutSetDto
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
                                IsSkipped = set.IsSkipped
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
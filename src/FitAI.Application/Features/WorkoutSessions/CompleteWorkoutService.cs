using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.WorkoutSessions;

public sealed class CompleteWorkoutService(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : ICompleteWorkoutService
{
    public async Task<CompleteWorkoutResult> CompleteAsync(
        CompleteWorkoutCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId = await currentUserService.GetUserIdAsync(
    cancellationToken);

        if (command.WorkoutSessionId == Guid.Empty)
        {
            throw new ArgumentException(
                "Workout session ID is required.",
                nameof(command));
        }

        var session = await context.WorkoutSessions
            .Include(x => x.Exercises)
                .ThenInclude(x => x.Sets)
            .FirstOrDefaultAsync(
                x => x.Id == command.WorkoutSessionId
                     && x.UserId == userId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Workout session not found.");

        if (session.Status == "Completed")
        {
            throw new InvalidOperationException(
                "Workout session is already completed.");
        }

        var allSets = session.Exercises
            .SelectMany(x => x.Sets)
            .ToList();

        var totalSets = allSets.Count;

        var completedSets = allSets.Count(
            x => x.IsCompleted && !x.IsSkipped);

        var totalVolume = allSets
            .Where(x =>
                x.IsCompleted
                && !x.IsSkipped
                && x.ActualWeightKg.HasValue
                && x.ActualReps.HasValue)
            .Sum(x =>
                x.ActualWeightKg!.Value
                * x.ActualReps!.Value);

        var completedAt = DateTimeOffset.UtcNow;

        var durationMinutes = session.StartTime.HasValue
            ? Math.Max(
                0,
                (int)Math.Round(
                    (completedAt - session.StartTime.Value)
                    .TotalMinutes))
            : 0;

        session.Status = "Completed";
        session.EndTime = completedAt;
        session.CompletedAt = completedAt;
        session.DurationMinutes = durationMinutes;
        session.TotalSets = totalSets;
        session.CompletedSets = completedSets;
        session.TotalVolume = totalVolume;
        session.UpdatedAt = completedAt;

        await context.SaveChangesAsync(cancellationToken);

        return new CompleteWorkoutResult
        {
            WorkoutSessionId = session.Id,
            TotalSets = totalSets,
            CompletedSets = completedSets,
            TotalVolume = totalVolume,
            DurationMinutes = durationMinutes,
            CompletedAt = completedAt
        };
    }
}
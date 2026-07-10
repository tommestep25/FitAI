namespace FitAI.Application.Features.WorkoutSessions;

public sealed class CompleteWorkoutResult
{
    public Guid WorkoutSessionId { get; init; }

    public int TotalSets { get; init; }

    public int CompletedSets { get; init; }

    public decimal TotalVolume { get; init; }

    public int DurationMinutes { get; init; }

    public DateTimeOffset CompletedAt { get; init; }
}
using FitAI.Domain.Common;

namespace FitAI.Domain.Entities;

public class WorkoutSet : BaseEntity
{
    public Guid WorkoutSessionExerciseId { get; set; }

    public int SetNumber { get; set; }

    public decimal? TargetWeightKg { get; set; }
    public decimal? ActualWeightKg { get; set; }

    public int? TargetReps { get; set; }
    public int? ActualReps { get; set; }

    public decimal? Rpe { get; set; }

    public int? TargetRestSeconds { get; set; }
    public int? ActualRestSeconds { get; set; }

    public string SetType { get; set; } = "Working";

    public bool IsWarmup { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsSkipped { get; set; }

    public string? Tempo { get; set; }
    public string? Feeling { get; set; }
    public string? Notes { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public WorkoutSessionExercise WorkoutSessionExercise { get; set; }
        = null!;
}
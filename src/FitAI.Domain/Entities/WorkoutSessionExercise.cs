using FitAI.Domain.Common;

namespace FitAI.Domain.Entities;

public class WorkoutSessionExercise : BaseEntity
{
    public Guid WorkoutSessionId { get; set; }
    public Guid ExerciseId { get; set; }

    public int SortOrder { get; set; }

    public int? TargetSets { get; set; }
    public int? TargetMinReps { get; set; }
    public int? TargetMaxReps { get; set; }
    public int? TargetRestSeconds { get; set; }
    public decimal? TargetRpe { get; set; }

    public bool IsCompleted { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }

    public string? Notes { get; set; }

    public WorkoutSession WorkoutSession { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;

    public ICollection<WorkoutSet> Sets { get; set; }
        = new List<WorkoutSet>();
}
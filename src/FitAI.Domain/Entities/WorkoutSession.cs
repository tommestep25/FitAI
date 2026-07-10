using FitAI.Domain.Common;

namespace FitAI.Domain.Entities;

public class WorkoutSession : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid? WorkoutPlanId { get; set; }
    public Guid? WorkoutDayId { get; set; }

    public Guid? SourceTemplateDayId { get; set; }

    public DateOnly SessionDate { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }

    public int? DurationMinutes { get; set; }
    public string? SessionName { get; set; }
    public string? Notes { get; set; }
    public short? Rating { get; set; }
    public string Status { get; set; } = "InProgress";

    public DateTimeOffset? CompletedAt { get; set; }

    public int TotalSets { get; set; }

    public int CompletedSets { get; set; }

    public decimal TotalVolume { get; set; }

    public ICollection<WorkoutSessionExercise> Exercises { get; set; }
        = new List<WorkoutSessionExercise>();
}
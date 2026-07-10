using FitAI.Domain.Common;

namespace FitAI.Domain.Entities;

public class WorkoutTemplateExercise : BaseEntity
{
    public Guid WorkoutTemplateDayId { get; set; }
    public Guid ExerciseId { get; set; }
    public int SortOrder { get; set; }
    public int TargetSets { get; set; }
    public int? TargetMinReps { get; set; }
    public int? TargetMaxReps { get; set; }
    public int? TargetRestSeconds { get; set; }
    public decimal? TargetRpe { get; set; }
    public string? Notes { get; set; }
}
namespace FitAI.Application.Features.WorkoutSessionDetail;

public sealed class WorkoutExerciseDto
{
    public Guid WorkoutSessionExerciseId { get; set; }
    public Guid ExerciseId { get; set; }

    public string ExerciseName { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public int? TargetSets { get; set; }
    public int? TargetMinReps { get; set; }
    public int? TargetMaxReps { get; set; }
    public int? TargetRestSeconds { get; set; }
    public decimal? TargetRpe { get; set; }

    public bool IsCompleted { get; set; }

    public IReadOnlyList<WorkoutSetDto> Sets { get; set; }
        = [];
}
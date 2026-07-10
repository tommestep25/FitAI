using FitAI.Domain.Common;

namespace FitAI.Domain.Entities;

public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid ExerciseCategoryId { get; set; }
    public Guid? EquipmentId { get; set; }

    public int? DefaultSets { get; set; }
    public int? DefaultMinReps { get; set; }
    public int? DefaultMaxReps { get; set; }
    public int? DefaultRestSeconds { get; set; }
    public string? Tempo { get; set; }

    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public ICollection<WorkoutSessionExercise> WorkoutSessionExercises
    { get; set; } = new List<WorkoutSessionExercise>();
}
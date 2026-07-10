using FitAI.Domain.Common;

namespace FitAI.Domain.Entities;

public class WorkoutTemplateDay : BaseEntity
{
    public Guid WorkoutPlanTemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DayType { get; set; }
    public int SortOrder { get; set; }
    public string? Notes { get; set; }
}
namespace FitAI.Application.Features.WorkoutTemplates;

public sealed class WorkoutTemplateDayDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? DayType { get; init; }
    public int SortOrder { get; init; }
}
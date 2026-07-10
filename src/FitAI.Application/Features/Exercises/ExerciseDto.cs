namespace FitAI.Application.Features.Exercises;

public sealed class ExerciseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? DefaultSets { get; init; }
    public int? DefaultMinReps { get; init; }
    public int? DefaultMaxReps { get; init; }
    public int? DefaultRestSeconds { get; init; }
    public string? Tempo { get; init; }
}
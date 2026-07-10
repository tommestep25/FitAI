namespace FitAI.Application.Features.WorkoutSessionDetail;

public sealed class WorkoutSessionDetailDto
{
    public Guid SessionId { get; init; }
    public string SessionName { get; init; } = string.Empty;
    public DateOnly SessionDate { get; init; }
    public DateTimeOffset? StartTime { get; init; }
    public string Status { get; init; } = "InProgress";

    public IReadOnlyList<WorkoutExerciseDto> Exercises { get; init; }
        = [];
}
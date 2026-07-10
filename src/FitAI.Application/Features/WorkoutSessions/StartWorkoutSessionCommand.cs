namespace FitAI.Application.Features.WorkoutSessions;

public sealed class StartWorkoutSessionCommand
{
    public Guid WorkoutTemplateDayId { get; init; }
}
namespace FitAI.Application.Features.WorkoutSessionDetail;

public sealed class WorkoutSetDto
{
    public Guid Id { get; set; }

    public int SetNumber { get; set; }

    public decimal? TargetWeightKg { get; set; }
    public decimal? ActualWeightKg { get; set; }

    public int? TargetReps { get; set; }
    public int? ActualReps { get; set; }

    public decimal? Rpe { get; set; }

    public int? TargetRestSeconds { get; set; }
    public int? ActualRestSeconds { get; set; }

    public string SetType { get; set; } = "Working";

    public bool IsWarmup { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsSkipped { get; set; }
}
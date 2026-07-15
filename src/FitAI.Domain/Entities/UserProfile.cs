namespace FitAI.Domain.Entities;

public sealed class UserProfile
{
    public Guid UserId { get; set; }

    public string? Email { get; set; }

    public string? DisplayName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public decimal? HeightCm { get; set; }

    public decimal? CurrentWeightKg { get; set; }

    public decimal? GoalWeightKg { get; set; }

    public string? ActivityLevel { get; set; }

    public string? FitnessGoal { get; set; }

    public string? AvatarUrl { get; set; }

    public string Timezone { get; set; } = "Asia/Bangkok";

    public bool IsActive { get; set; } = true;

    public bool OnboardingCompleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}
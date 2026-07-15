using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration
    : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(
        EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles", "app");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(255);

        builder.Property(x => x.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(150);

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100);

        builder.Property(x => x.BirthDate)
            .HasColumnName("birth_date");

        builder.Property(x => x.Gender)
            .HasColumnName("gender")
            .HasMaxLength(30);

        builder.Property(x => x.HeightCm)
            .HasColumnName("height_cm")
            .HasPrecision(5, 2);

        builder.Property(x => x.CurrentWeightKg)
            .HasColumnName("current_weight_kg")
            .HasPrecision(5, 2);

        builder.Property(x => x.GoalWeightKg)
            .HasColumnName("goal_weight_kg")
            .HasPrecision(5, 2);

        builder.Property(x => x.ActivityLevel)
            .HasColumnName("activity_level")
            .HasMaxLength(50);

        builder.Property(x => x.FitnessGoal)
            .HasColumnName("fitness_goal")
            .HasMaxLength(100);

        builder.Property(x => x.AvatarUrl)
            .HasColumnName("avatar_url");

        builder.Property(x => x.Timezone)
            .HasColumnName("timezone")
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active");

        builder.Property(x => x.OnboardingCompleted)
            .HasColumnName("onboarding_completed");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
    }
}
using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public sealed class WorkoutSessionConfiguration
    : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("workout_sessions", "workout");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.WorkoutPlanId)
            .HasColumnName("workout_plan_id");

        builder.Property(x => x.WorkoutDayId)
            .HasColumnName("workout_day_id");

        builder.Property(x => x.SourceTemplateDayId)
            .HasColumnName("source_template_day_id");

        builder.Property(x => x.SessionDate)
            .HasColumnName("session_date");

        builder.Property(x => x.StartTime)
            .HasColumnName("start_time");

        builder.Property(x => x.EndTime)
            .HasColumnName("end_time");

        builder.Property(x => x.DurationMinutes)
            .HasColumnName("duration_minutes");

        builder.Property(x => x.SessionName)
            .HasColumnName("session_name")
            .HasMaxLength(150);

        builder.Property(x => x.Notes)
            .HasColumnName("notes");

        builder.Property(x => x.Rating)
            .HasColumnName("rating");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
        builder.Property(x => x.Status)
            .HasColumnName("status");

        builder.Property(x => x.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(x => x.TotalSets)
            .HasColumnName("total_sets");

        builder.Property(x => x.CompletedSets)
            .HasColumnName("completed_sets");

        builder.Property(x => x.TotalVolume)
            .HasColumnName("total_volume")
            .HasPrecision(12, 2);

        builder.HasMany(x => x.Exercises)
            .WithOne(x => x.WorkoutSession)
            .HasForeignKey(x => x.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
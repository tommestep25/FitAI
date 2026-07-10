using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public sealed class WorkoutSetConfiguration
    : IEntityTypeConfiguration<WorkoutSet>
{
    public void Configure(EntityTypeBuilder<WorkoutSet> builder)
    {
        builder.ToTable("workout_sets", "workout");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.WorkoutSessionExerciseId)
            .HasColumnName("workout_session_exercise_id");

        builder.Property(x => x.SetNumber)
            .HasColumnName("set_number");

        builder.Property(x => x.TargetWeightKg)
            .HasColumnName("target_weight_kg")
            .HasPrecision(7, 2);

        builder.Property(x => x.ActualWeightKg)
            .HasColumnName("actual_weight_kg")
            .HasPrecision(7, 2);

        builder.Property(x => x.TargetReps)
            .HasColumnName("target_reps");

        builder.Property(x => x.ActualReps)
            .HasColumnName("actual_reps");

        builder.Property(x => x.Rpe)
            .HasColumnName("rpe")
            .HasPrecision(3, 1);

        builder.Property(x => x.TargetRestSeconds)
            .HasColumnName("target_rest_seconds");

        builder.Property(x => x.ActualRestSeconds)
            .HasColumnName("actual_rest_seconds");

        builder.Property(x => x.SetType)
            .HasColumnName("set_type")
            .HasMaxLength(30);

        builder.Property(x => x.IsWarmup)
            .HasColumnName("is_warmup");

        builder.Property(x => x.IsCompleted)
            .HasColumnName("is_completed");

        builder.Property(x => x.IsSkipped)
            .HasColumnName("is_skipped");

        builder.Property(x => x.Tempo)
            .HasColumnName("tempo")
            .HasMaxLength(50);

        builder.Property(x => x.Feeling)
            .HasColumnName("feeling")
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasColumnName("notes");

        builder.Property(x => x.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(x => x.WorkoutSessionExercise)
            .WithMany(x => x.Sets)
            .HasForeignKey(x => x.WorkoutSessionExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.WorkoutSessionExerciseId,
            x.SetNumber
        })
        .IsUnique();
    }
}
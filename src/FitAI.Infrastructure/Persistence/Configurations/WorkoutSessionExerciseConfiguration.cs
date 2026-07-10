using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public sealed class WorkoutSessionExerciseConfiguration
    : IEntityTypeConfiguration<WorkoutSessionExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutSessionExercise> builder)
    {
        builder.ToTable("workout_session_exercises", "workout");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.WorkoutSessionId)
            .HasColumnName("workout_session_id");

        builder.Property(x => x.ExerciseId)
            .HasColumnName("exercise_id");

        builder.Property(x => x.SortOrder)
            .HasColumnName("sort_order");

        builder.Property(x => x.Notes)
            .HasColumnName("notes");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
        builder.Property(x => x.TargetSets)
    .HasColumnName("target_sets");

        builder.Property(x => x.TargetMinReps)
            .HasColumnName("target_min_reps");

        builder.Property(x => x.TargetMaxReps)
            .HasColumnName("target_max_reps");

        builder.Property(x => x.TargetRestSeconds)
            .HasColumnName("target_rest_seconds");

        builder.Property(x => x.TargetRpe)
            .HasColumnName("target_rpe")
            .HasPrecision(3, 1);

        builder.Property(x => x.IsCompleted)
            .HasColumnName("is_completed");

        builder.Property(x => x.CompletedAt)
            .HasColumnName("completed_at");

        builder.HasOne(x => x.WorkoutSession)
            .WithMany(x => x.Exercises)
            .HasForeignKey(x => x.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.WorkoutSessionExercises)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Sets)
            .WithOne(x => x.WorkoutSessionExercise)
            .HasForeignKey(x => x.WorkoutSessionExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
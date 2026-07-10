using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public class WorkoutTemplateExerciseConfiguration : IEntityTypeConfiguration<WorkoutTemplateExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutTemplateExercise> builder)
    {
        builder.ToTable("workout_template_exercises", "workout");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.WorkoutTemplateDayId).HasColumnName("workout_template_day_id");
        builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");
        builder.Property(x => x.SortOrder).HasColumnName("sort_order");
        builder.Property(x => x.TargetSets).HasColumnName("target_sets");
        builder.Property(x => x.TargetMinReps).HasColumnName("target_min_reps");
        builder.Property(x => x.TargetMaxReps).HasColumnName("target_max_reps");
        builder.Property(x => x.TargetRestSeconds).HasColumnName("target_rest_seconds");
        builder.Property(x => x.TargetRpe).HasColumnName("target_rpe").HasPrecision(3, 1);
        builder.Property(x => x.Notes).HasColumnName("notes");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }
}
using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises", "workout");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.ExerciseCategoryId).HasColumnName("exercise_category_id");
        builder.Property(x => x.EquipmentId).HasColumnName("equipment_id");
        builder.Property(x => x.DefaultSets).HasColumnName("default_sets");
        builder.Property(x => x.DefaultMinReps).HasColumnName("default_min_reps");
        builder.Property(x => x.DefaultMaxReps).HasColumnName("default_max_reps");
        builder.Property(x => x.DefaultRestSeconds).HasColumnName("default_rest_seconds");
        builder.Property(x => x.Tempo).HasColumnName("tempo").HasMaxLength(50);
        builder.Property(x => x.ImageUrl).HasColumnName("image_url");
        builder.Property(x => x.VideoUrl).HasColumnName("video_url");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
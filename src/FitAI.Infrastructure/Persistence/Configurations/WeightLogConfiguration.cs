using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public class WeightLogConfiguration : IEntityTypeConfiguration<WeightLog>
{
    public void Configure(EntityTypeBuilder<WeightLog> builder)
    {
        builder.ToTable("weight_logs", "progress");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.WeightKg).HasColumnName("weight_kg").HasPrecision(5, 2);
        builder.Property(x => x.BodyFatPercentage).HasColumnName("body_fat_percentage").HasPrecision(5, 2);
        builder.Property(x => x.SkeletalMuscleMassKg).HasColumnName("skeletal_muscle_mass_kg").HasPrecision(5, 2);
        builder.Property(x => x.Bmi).HasColumnName("bmi").HasPrecision(5, 2);
        builder.Property(x => x.Bmr).HasColumnName("bmr");
        builder.Property(x => x.MeasuredAt).HasColumnName("measured_at");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => new { x.UserId, x.MeasuredAt });
    }
}
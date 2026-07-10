using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public class InBodyLogConfiguration : IEntityTypeConfiguration<InBodyLog>
{
    public void Configure(EntityTypeBuilder<InBodyLog> builder)
    {
        builder.ToTable("inbody_logs", "progress");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.ScanDate).HasColumnName("scan_date");
        builder.Property(x => x.WeightKg).HasColumnName("weight_kg").HasPrecision(5, 2);
        builder.Property(x => x.SkeletalMuscleMassKg).HasColumnName("skeletal_muscle_mass_kg").HasPrecision(5, 2);
        builder.Property(x => x.BodyFatMassKg).HasColumnName("body_fat_mass_kg").HasPrecision(5, 2);
        builder.Property(x => x.PercentBodyFat).HasColumnName("percent_body_fat").HasPrecision(5, 2);
        builder.Property(x => x.Bmi).HasColumnName("bmi").HasPrecision(5, 2);
        builder.Property(x => x.Bmr).HasColumnName("bmr");
        builder.Property(x => x.VisceralFatLevel).HasColumnName("visceral_fat_level").HasPrecision(5, 2);
        builder.Property(x => x.InBodyScore).HasColumnName("inbody_score");
        builder.Property(x => x.Memo).HasColumnName("memo");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => new { x.UserId, x.ScanDate });
    }
}
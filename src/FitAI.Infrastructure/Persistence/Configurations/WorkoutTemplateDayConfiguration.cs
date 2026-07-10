using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitAI.Infrastructure.Persistence.Configurations;

public class WorkoutTemplateDayConfiguration : IEntityTypeConfiguration<WorkoutTemplateDay>
{
    public void Configure(EntityTypeBuilder<WorkoutTemplateDay> builder)
    {
        builder.ToTable("workout_template_days", "workout");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.WorkoutPlanTemplateId).HasColumnName("workout_plan_template_id");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.DayType).HasColumnName("day_type").HasMaxLength(50);
        builder.Property(x => x.SortOrder).HasColumnName("sort_order");
        builder.Property(x => x.Notes).HasColumnName("notes");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }
}
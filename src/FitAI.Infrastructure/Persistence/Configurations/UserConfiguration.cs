using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", "auth_app");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnName("password_hash");

            builder.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100);

            builder.Property(x => x.BirthDate)
                .HasColumnName("birth_date");

            builder.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasMaxLength(20);

            builder.Property(x => x.HeightCm)
                .HasColumnName("height_cm")
                .HasPrecision(5, 2);

            builder.Property(x => x.CurrentWeightKg)
                .HasColumnName("current_weight_kg")
                .HasPrecision(5, 2);

            builder.Property(x => x.GoalWeightKg)
                .HasColumnName("goal_weight_kg")
                .HasPrecision(5, 2);

            builder.Property(x => x.ProfileImageUrl)
                .HasColumnName("profile_image_url");

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted");

            builder.Property(x => x.DeletedAt)
                .HasColumnName("deleted_at");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}

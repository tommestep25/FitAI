using FitAI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }

        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }

        public decimal? HeightCm { get; set; }
        public decimal? CurrentWeightKg { get; set; }
        public decimal? GoalWeightKg { get; set; }

        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}

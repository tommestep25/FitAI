using FitAI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Domain.Entities
{
    public class WeightLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal WeightKg { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? SkeletalMuscleMassKg { get; set; }
        public decimal? Bmi { get; set; }
        public int? Bmr { get; set; }
        public DateTimeOffset MeasuredAt { get; set; }
        public string? Note { get; set; }
    }
}

using FitAI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Domain.Entities
{
    public class InBodyLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateOnly ScanDate { get; set; }
        public decimal WeightKg { get; set; }
        public decimal? SkeletalMuscleMassKg { get; set; }
        public decimal? BodyFatMassKg { get; set; }
        public decimal? PercentBodyFat { get; set; }
        public decimal? Bmi { get; set; }
        public int? Bmr { get; set; }
        public decimal? VisceralFatLevel { get; set; }
        public int? InBodyScore { get; set; }
        public string? Memo { get; set; }
    }
}

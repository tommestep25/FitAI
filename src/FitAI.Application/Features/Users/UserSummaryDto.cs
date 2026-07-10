using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Application.Features.Users
{
    public sealed class UserSummaryDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public decimal? CurrentWeightKg { get; init; }
        public decimal? GoalWeightKg { get; init; }
    }
}

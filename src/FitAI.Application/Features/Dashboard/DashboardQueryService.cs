using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.Dashboard;

public sealed class DashboardQueryService(IApplicationDbContext context) : IDashboardQueryService
{
    public async Task<DashboardSummaryDto?> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.IsActive)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;

        var latestInBody = await context.InBodyLogs
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.ScanDate)
            .FirstOrDefaultAsync(cancellationToken);

        return new DashboardSummaryDto
        {
            FirstName = user.FirstName,
            CurrentWeightKg = user.CurrentWeightKg,
            GoalWeightKg = user.GoalWeightKg,
            LatestInBodyWeightKg = latestInBody?.WeightKg,
            LatestBodyFatPercentage = latestInBody?.PercentBodyFat,
            LatestSkeletalMuscleMassKg = latestInBody?.SkeletalMuscleMassKg,
            LatestBmr = latestInBody?.Bmr,
            LatestInBodyScore = latestInBody?.InBodyScore
        };
    }
}
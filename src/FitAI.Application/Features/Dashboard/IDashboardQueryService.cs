namespace FitAI.Application.Features.Dashboard;

public interface IDashboardQueryService
{
    Task<DashboardSummaryDto?> GetDashboardAsync(CancellationToken cancellationToken = default);
}
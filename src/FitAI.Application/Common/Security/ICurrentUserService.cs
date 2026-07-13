namespace FitAI.Application.Common.Security;

public interface ICurrentUserService
{
    Task<bool> IsAuthenticatedAsync(
        CancellationToken cancellationToken = default);

    Task<Guid?> GetUserIdAsync(
        CancellationToken cancellationToken = default);

    Task<Guid> GetRequiredUserIdAsync(
        CancellationToken cancellationToken = default);

    Task<string?> GetEmailAsync(
        CancellationToken cancellationToken = default);
}
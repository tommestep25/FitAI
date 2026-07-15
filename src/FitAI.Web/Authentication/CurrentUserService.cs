using System.Security.Claims;
using FitAI.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace FitAI.Web.Authentication;

public sealed class CurrentUserService(
    AuthenticationStateProvider authenticationStateProvider)
    : ICurrentUserService
{
    public async Task<bool> IsAuthenticatedAsync(
        CancellationToken cancellationToken = default)
    {
        var principal = await GetPrincipalAsync();
        return principal.Identity?.IsAuthenticated == true;
    }

    public async Task<Guid> GetUserIdAsync(
        CancellationToken cancellationToken = default)
    {
        var principal = await GetPrincipalAsync();

        if (principal.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        var userIdValue =
            principal.FindFirstValue(
                ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(
                "supabase_user_id");

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new InvalidOperationException(
                "Authenticated user ID is missing or invalid.");
        }

        return userId;
    }

    public async Task<string?> GetEmailAsync(
        CancellationToken cancellationToken = default)
    {
        var principal = await GetPrincipalAsync();

        return principal.FindFirstValue(ClaimTypes.Email)
            ?? principal.Identity?.Name;
    }

    private async Task<ClaimsPrincipal> GetPrincipalAsync()
    {
        var authenticationState =
            await authenticationStateProvider
                .GetAuthenticationStateAsync();

        return authenticationState.User;
    }
}
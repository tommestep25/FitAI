using System.Security.Claims;
using FitAI.Application.Common.Security;
using Microsoft.AspNetCore.Components.Authorization;

namespace FitAI.Web.Authentication;

public sealed class CurrentUserService(
    AuthenticationStateProvider authenticationStateProvider)
    : ICurrentUserService
{
    public bool IsAuthenticated =>
        GetPrincipal().Identity?.IsAuthenticated == true;

    public Guid? UserId
    {
        get
        {
            var value = GetPrincipal().FindFirstValue(
                ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string? Email =>
        GetPrincipal().FindFirstValue(ClaimTypes.Email);

    public Guid GetRequiredUserId()
    {
        return UserId
            ?? throw new UnauthorizedAccessException(
                "ไม่พบผู้ใช้ที่เข้าสู่ระบบ");
    }

    private ClaimsPrincipal GetPrincipal()
    {
        var state = authenticationStateProvider
            .GetAuthenticationStateAsync()
            .GetAwaiter()
            .GetResult();

        return state.User;
    }
}
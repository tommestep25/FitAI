using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Features.Profiles;

public sealed class CurrentUserProfileService(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : ICurrentUserProfileService
{
    public async Task<CurrentUserProfileDto?> GetCurrentAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = await currentUserService.GetUserIdAsync(
            cancellationToken);

        return await context.UserProfiles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new CurrentUserProfileDto
            {
                UserId = x.UserId,
                Email = x.Email,
                DisplayName = x.DisplayName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                HeightCm = x.HeightCm,
                CurrentWeightKg = x.CurrentWeightKg,
                GoalWeightKg = x.GoalWeightKg,
                ActivityLevel = x.ActivityLevel,
                FitnessGoal = x.FitnessGoal,
                Timezone = x.Timezone,
                IsActive = x.IsActive,
                OnboardingCompleted =
                    x.OnboardingCompleted
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsOnboardingCompletedAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = await currentUserService.GetUserIdAsync(
            cancellationToken);

        return await context.UserProfiles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.OnboardingCompleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
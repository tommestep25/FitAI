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
    public async Task<CompleteOnboardingResult> CompleteOnboardingAsync(
    CompleteOnboardingCommand command,
    CancellationToken cancellationToken = default)
    {
        ValidateOnboarding(command);

        var userId = await currentUserService.GetUserIdAsync(
            cancellationToken);

        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "User profile not found.");

        if (!profile.IsActive)
        {
            throw new UnauthorizedAccessException(
                "User profile is inactive.");
        }

        var now = DateTimeOffset.UtcNow;

        profile.BirthDate = command.BirthDate;
        profile.Gender = command.Gender.Trim();
        profile.HeightCm = command.HeightCm;
        profile.CurrentWeightKg = command.CurrentWeightKg;
        profile.GoalWeightKg = command.GoalWeightKg;
        profile.ActivityLevel = command.ActivityLevel.Trim();
        profile.FitnessGoal = command.FitnessGoal.Trim();
        profile.Timezone = string.IsNullOrWhiteSpace(command.Timezone)
            ? "Asia/Bangkok"
            : command.Timezone.Trim();

        profile.OnboardingCompleted = true;
        profile.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);

        return new CompleteOnboardingResult
        {
            UserId = profile.UserId,
            OnboardingCompleted = true,
            UpdatedAt = now
        };
    }
    private static void ValidateOnboarding(
    CompleteOnboardingCommand command)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (command.BirthDate >= today)
        {
            throw new ArgumentException(
                "Birth date must be earlier than today.");
        }

        var minimumBirthDate = today.AddYears(-120);

        if (command.BirthDate < minimumBirthDate)
        {
            throw new ArgumentException(
                "Birth date is outside the supported range.");
        }

        if (string.IsNullOrWhiteSpace(command.Gender))
        {
            throw new ArgumentException(
                "Gender is required.");
        }

        if (command.HeightCm is < 50 or > 300)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.HeightCm),
                "Height must be between 50 and 300 cm.");
        }

        if (command.CurrentWeightKg is < 20 or > 500)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.CurrentWeightKg),
                "Current weight must be between 20 and 500 kg.");
        }

        if (command.GoalWeightKg is < 20 or > 500)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.GoalWeightKg),
                "Goal weight must be between 20 and 500 kg.");
        }

        if (string.IsNullOrWhiteSpace(command.ActivityLevel))
        {
            throw new ArgumentException(
                "Activity level is required.");
        }

        if (string.IsNullOrWhiteSpace(command.FitnessGoal))
        {
            throw new ArgumentException(
                "Fitness goal is required.");
        }
    }
}
using FitAI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Application.Features.Users
{
    public sealed class UserQueryService(IApplicationDbContext context) : IUserQueryService
    {
        public async Task<IReadOnlyList<UserSummaryDto>> GetUsersAsync(
            CancellationToken cancellationToken = default)
        {
            return await context.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new UserSummaryDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    CurrentWeightKg = x.CurrentWeightKg,
                    GoalWeightKg = x.GoalWeightKg
                })
                .ToListAsync(cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Application.Features.Users
{
    public interface IUserQueryService
    {
        Task<IReadOnlyList<UserSummaryDto>> GetUsersAsync(
            CancellationToken cancellationToken = default);
    }
}

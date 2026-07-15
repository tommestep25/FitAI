using System;
using System.Collections.Generic;
using System.Text;

namespace FitAI.Application.Features.Authentication
{
    public sealed class ResendConfirmationCommand
    {
        public string Email { get; init; } = string.Empty;
    }
}

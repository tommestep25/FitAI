namespace FitAI.Application.Features.Authentication;

public sealed class AuthServiceException : Exception
{
    public AuthServiceException(
        string message,
        int? statusCode = null,
        string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public int? StatusCode { get; }

    public string? ErrorCode { get; }
}
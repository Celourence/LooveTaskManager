using System.Net;

namespace LooveTaskManager.Domain.Common.Exceptions;

public class CustomException : Exception
{
    public string ErrorCode { get; }
    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, string errorCode, HttpStatusCode statusCode) 
        : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    public CustomException(string message, string errorCode, HttpStatusCode statusCode, Exception innerException) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
} 
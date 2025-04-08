using System.Net;
using System.Text.Json;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.API.Models;
using LooveTaskManager.Application.Constants;
using System.Text;

namespace LooveTaskManager.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/problem+json";

        var errorDetail = new StringBuilder();
        errorDetail.AppendLine(exception.Message);

        if (_environment.IsDevelopment())
        {
            errorDetail.AppendLine();
            errorDetail.AppendLine("Stack Trace:");
            errorDetail.AppendLine(exception.StackTrace);

            if (exception.InnerException != null)
            {
                errorDetail.AppendLine();
                errorDetail.AppendLine("Inner Exception:");
                errorDetail.AppendLine(exception.InnerException.Message);
                errorDetail.AppendLine(exception.InnerException.StackTrace);
            }
        }

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Erro interno do servidor",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = errorDetail.ToString(),
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case CustomException customEx:
                _logger.LogWarning(string.Format(Messages.Log.Error.DomainError, exception.Message, customEx.StatusCode));
                problemDetails.Status = (int)customEx.StatusCode;
                problemDetails.Title = customEx.Message;
                problemDetails.Extensions["errorCode"] = customEx.ErrorCode;
                break;

            case InvalidOperationException:
                _logger.LogWarning(string.Format(Messages.Log.Error.DomainError, exception.Message, HttpStatusCode.BadRequest));
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Erro de validação";
                problemDetails.Extensions["errorCode"] = "VALIDATION_ERROR";
                break;

            case KeyNotFoundException:
                _logger.LogWarning(string.Format(Messages.Log.Error.NotFoundError, exception.Message));
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                problemDetails.Title = "Recurso não encontrado";
                problemDetails.Extensions["errorCode"] = "NOT_FOUND";
                break;

            case UnauthorizedAccessException:
                _logger.LogWarning(string.Format(Messages.Log.Error.DomainError, exception.Message, HttpStatusCode.Unauthorized));
                problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = "Não autorizado";
                problemDetails.Extensions["errorCode"] = "UNAUTHORIZED";
                break;

            default:
                _logger.LogError(exception, string.Format(Messages.Log.Error.UnhandledError, exception.Message));
                problemDetails.Extensions["errorCode"] = "INTERNAL_SERVER_ERROR";
                break;
        }

        response.StatusCode = problemDetails.Status;

        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonOptions));
    }
} 
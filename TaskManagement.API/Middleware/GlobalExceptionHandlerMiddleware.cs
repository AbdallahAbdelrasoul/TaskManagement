using System.Text.Json;
using TaskManagement.Domain.Shared.Exceptions;
using TaskManagement.Domain.Shared.Models;

namespace TaskManagement.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        context.Response.ContentType = "application/json";

        int statusCode;
        object body;

        switch (exception)
        {
            case NotFoundException nfe:
                statusCode = StatusCodes.Status404NotFound;
                body = ApiResponse<object>.Fail(nfe.Message);
                break;

            case Domain.Shared.Exceptions.ValidationException ve:
                statusCode = StatusCodes.Status422UnprocessableEntity;
                body = ApiResponse<object>.Fail("Validation failed.", ve.Errors);
                break;

            case UnauthorizedException ue:
                statusCode = StatusCodes.Status403Forbidden;
                body = ApiResponse<object>.Fail(ue.Message);
                break;

            case DuplicateException de:
                statusCode = StatusCodes.Status409Conflict;
                body = ApiResponse<object>.Fail(de.Message);
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                body = ApiResponse<object>.Fail("An unexpected error occurred.");
                break;
        }

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}

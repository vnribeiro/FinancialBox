using FinancialBox.Domain.Primitives;
using FinancialBox.Presentation.Responses;
using System.Text.Json;

namespace FinancialBox.Presentation.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception on {Method} {Path}", context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var error = Error.InternalError("General.UnexpectedError", env.IsDevelopment() ? exception.Message : "An unexpected error occurred.");

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var response = ApiProblemDetails.FromErrors([error], StatusCodes.Status500InternalServerError);
        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

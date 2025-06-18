using FinancialBox.Presentation.Responses;
using System.Text.Json;

namespace FinancialBox.Presentation.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, 
        ILogger<ErrorHandlingMiddleware> logger, 
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while processing request.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var message = _env.IsDevelopment()
            ? exception.Message
            : "An unexpected error occurred.";

        var response = ApiResponse<string>.FromErrors([message]);
        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
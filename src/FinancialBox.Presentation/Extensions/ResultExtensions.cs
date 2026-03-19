using FinancialBox.Domain.Primitives;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult Match(
        this Result result,
        Func<ActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess();

        var statusCode = ToStatusCode(result.Errors[0].Type);
        return new ObjectResult(ApiProblemDetails.FromErrors(result.Errors, statusCode))
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }

    public static ActionResult<ApiResponse<T>> Match<T>(
        this Result<T> result,
        Func<ApiResponse<T>, ActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(ApiResponse<T>.Success(result.Data));

        var statusCode = ToStatusCode(result.Errors[0].Type);
        return new ObjectResult(ApiProblemDetails.FromErrors(result.Errors, statusCode))
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }

    public static ActionResult ToProblem(this Error error)
    {
        var statusCode = ToStatusCode(error.Type);
        return new ObjectResult(ApiProblemDetails.FromErrors([error], statusCode))
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }

    private static int ToStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation         => StatusCodes.Status422UnprocessableEntity,
        ErrorType.BusinessRule       => StatusCodes.Status400BadRequest,
        ErrorType.Unauthenticated    => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden          => StatusCodes.Status403Forbidden,
        ErrorType.NotFound           => StatusCodes.Status404NotFound,
        ErrorType.Conflict           => StatusCodes.Status409Conflict,
        ErrorType.TooManyRequests    => StatusCodes.Status429TooManyRequests,
        ErrorType.InternalError      => StatusCodes.Status500InternalServerError,
        ErrorType.NotImplemented     => StatusCodes.Status501NotImplemented,
        ErrorType.ServiceUnavailable => StatusCodes.Status503ServiceUnavailable,
        _                            => StatusCodes.Status500InternalServerError
    };
}

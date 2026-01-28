using FinancialBox.Application.Common;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult<ApiResponse<T>> Match<T>(
        this Result<T> result,
        Func<ApiResponse<T>, ActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(ApiResponse<T>.FromSuccess(result.Data!));

        var errorResponse = ApiResponse<T>.FromErrors(result.Error?.Messages ?? ["Unknown error"]);

        return new ObjectResult(errorResponse)
        {
            StatusCode = result.Error?.StatusCode ?? 500
        };
    }

    public static ActionResult Match(
        this Result result,
        Func<ActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess();

        var errorMessages = result.Error?.Messages ?? ["Unknown error"];
        var statusCode = result.Error?.StatusCode ?? 500;
        var errorResponse = ApiResponse.FromErrors(errorMessages);

        return new ObjectResult(errorResponse)
        {
            StatusCode = statusCode
        };
    }
}

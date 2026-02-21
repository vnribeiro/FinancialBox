using FinancialBox.Application.Common;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult<ApiResponse<T>> Match<T>(
        this Result<T> result,
        Func<ApiResponse<T>, ActionResult> onSuccess,
        Func<Error, ActionResult>? onError = null)
    {
        if (result.IsSuccess)
            return onSuccess(ApiResponse<T>.FromSuccess(result.Data!));

        if (onError != null)
            return onError(result.Error!);

        var errorResponse = ApiResponse<T>.FromErrors(result.Error?.Messages ?? ["Unknown error"]);
        
        return new ObjectResult(errorResponse)
        {
            StatusCode = result.Error?.StatusCode ?? 500
        };
    }
}

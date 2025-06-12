using FinancialBox.BuildingBlocks.Result;
using FinancialBox.Presentation.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult<ApiResponse<T>> ToApiResponseResult<T>(
        this Result<T> result,
        Func<ApiResponse<T>, ActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(ApiResponse<T>.FromSuccess(result.Value!));

        var errorResponse = ApiResponse<T>.FromErrors(result.Error?.Messages ?? ["Unknown error"]);

        return new ObjectResult(errorResponse)
        {
            StatusCode = result.Error?.StatusCode ?? 500
        };
    }

    public static ActionResult ToActionResult<T>(
        this Result<T> result,
        Func<T, ActionResult> onSuccess)
    {
        if(result.IsSuccess)
            return onSuccess(result.Value!);

        var errorResponse = ApiResponse<T>.FromErrors(result.Error?.Messages ?? ["Unknown error"]);

        return new ObjectResult(errorResponse)
        {
            StatusCode = result.Error?.StatusCode ?? 500
        };
    }
}

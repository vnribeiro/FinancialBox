using FinancialBox.BuildingBlocks.Result;
using FinancialBox.Presentation.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult<ApiResponse<T>> Match<T>(
        this Result<T> result,
        Func<ApiResponse<T>, ActionResult> onSuccess,
        Func<ApiResponse<IReadOnlyList<string>>, ActionResult> onFailure)
    {
        return result.IsSuccess ?
            onSuccess(ApiResponse<T>.FromSuccess(result.Value!)) :
            onFailure(ApiResponse<IReadOnlyList<string>>.FromErrors(result.Error?.Messages ?? ["Unknown error"]));
    }
}
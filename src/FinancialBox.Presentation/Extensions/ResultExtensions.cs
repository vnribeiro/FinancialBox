using FinancialBox.BuildingBlocks.Result;
using FinancialBox.Presentation.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult<ApiResponse<T>> MatchApiResponse<T>(
        this Result<T> result,
        Func<ApiResponse<T>, ActionResult> onSuccess,
        Func<ApiResponse<T>, ActionResult> onFailure)
    {
        return result.IsSuccess ? 
            onSuccess(ApiResponse<T>.FromSuccess(result.Value!)) : 
            onFailure((ApiResponse<T>.FromErrors(result.Error?.Messages ?? ["Unknown error"])));
    }

    public static ActionResult MatchPlain<T>(
        this Result<T> result,
        Func<T, ActionResult> onSuccess,
        Func<IReadOnlyList<string>, ActionResult> onFailure)
    {
        return result.IsSuccess ? 
            onSuccess(result.Value!) : 
            onFailure(result.Error?.Messages ?? new List<string> { "Unknown error" });
    }
}

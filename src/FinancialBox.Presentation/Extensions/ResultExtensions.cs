using FinancialBox.BuildingBlocks.Result;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Extensions;

public static class ResultExtensions
{
    public static ActionResult Match<T>(
        this Result<T> result,
        Func<T, ActionResult> onSuccess,
        Func<IReadOnlyList<string>, ActionResult> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value!)
            : onFailure(result.Error?.Messages ?? ["Unknown error"]);
    }
}

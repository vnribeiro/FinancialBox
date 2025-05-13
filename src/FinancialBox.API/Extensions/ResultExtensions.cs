using FinancialBox.API.Contracts;
using FinancialBox.Shared.ResultObjects;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.API.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult<ApiResponse<T>> Match<T>(
            this Result<T> result,
            Func<T, ActionResult<ApiResponse<T>>> onSuccess,
            Func<IReadOnlyList<string>, ActionResult<ApiResponse<T>>> onFailure)
        {
            return result.IsSuccess
                ? onSuccess(result.Value!)
                : onFailure(result.Error?.Messages ?? ["Unknown error"]);
        }
    }
}

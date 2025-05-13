namespace FinancialBox.Shared.ResultObjects
{
    public static class ResultExtensions
    {
        public static TResult Match<T, TResult>(
            this Result<T> result,
            Func<T, TResult> onSuccess,
            Func<IReadOnlyList<string>, TResult> onFailure)
        {
            return result.IsSuccess
                ? onSuccess(result.Value!)
                : onFailure(result.Error?.Messages ?? new List<string> { "Unknown error" });
        }
    }
}

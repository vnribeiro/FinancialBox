namespace FinancialBox.Shared.ResultObjects
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public Error? Error { get; }

        private Result(T? value, bool isSuccess, Error? error = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(value, true);
        public static Result<T> Failure(string message) => new(default, false, new Error(message));
        public static Result<T> Failure(IEnumerable<string> messages) => new(default, false, new Error(messages.ToArray()));
        public static Result<T> Failure(Error error) => new(default, false, error);
    }
}

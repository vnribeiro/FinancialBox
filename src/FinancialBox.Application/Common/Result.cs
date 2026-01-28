namespace FinancialBox.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    private Result(bool isSuccess, Error? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => 
        new(true);

    public static Result Failure(string message) => 
        new(false, Error.InvalidRequest(message));

    public static Result Failure(IEnumerable<string> messages) => 
        new(false, Error.InvalidRequest(messages.ToArray()));

    public static Result Failure(Error error) => 
        new(false, error);
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Data { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, T? data, Error? error = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static Result<T> Success(T data) =>
        new(true, data);

    public static Result<T> Failure(string message) =>
        new(false, default, Error.InvalidRequest(message));

    public static Result<T> Failure(IEnumerable<string> messages) =>
        new(false, default, Error.InvalidRequest(messages.ToArray()));

    public static Result<T> Failure(Error error) =>
        new(false, default, error);
}

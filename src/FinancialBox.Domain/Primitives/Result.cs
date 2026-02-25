namespace FinancialBox.Domain.Primitives;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<Error> Errors { get; }

    protected Result()
    {
        IsSuccess = true;
        Errors = [];
    }

    protected Result(IReadOnlyList<Error> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    public static Result Success() => new();
    public static Result Failure(Error error) => new([error]);
    public static Result Failure(IReadOnlyList<Error> errors) => new(errors);

    public static implicit operator Result(Error error) => new([error]);
}

public class Result<T> : Result
{
    private readonly T _data;

    private Result(T data)
    {
        _data = data;
    }

    private Result(IReadOnlyList<Error> errors) : base(errors)
    {
        _data = default!;
    }

    public T Data => IsSuccess ? _data : 
        throw new InvalidOperationException("Cannot access Data on a failed result.");

    public static implicit operator Result<T>(Error error) =>
        new([error]);

    public static implicit operator Result<T>(T data) =>
        new(data);

    public static Result<T> Success(T data) =>
        new(data);

    public new static Result<T> Failure(Error error) =>
        new([error]);

    public new static Result<T> Failure(IReadOnlyList<Error> errors) =>
        new(errors);
}

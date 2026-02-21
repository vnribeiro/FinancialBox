namespace FinancialBox.Application.Common;

public interface IResult<TSelf> where TSelf : IResult<TSelf>
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    Error? Error { get; }

    static abstract TSelf Failure(Error error);
}

using FinancialBox.Domain.Primitives;

namespace FinancialBox.UnitTests.Domain.Primitives;

public class ResultTests
{
    private static readonly Error SampleError = Error.Validation("Test.Error", "Something went wrong.");

    [Fact]
    public void Should_BeSuccess_When_ResultSuccessCreated()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_BeFailure_When_ResultFailureCreated()
    {
        var result = Result.Failure(SampleError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal("Test.Error", result.Errors[0].Code);
    }

    [Fact]
    public void Should_BeFailure_When_MultipleErrorsProvided()
    {
        var errors = new[] { SampleError, Error.NotFound("Other.Error", "Not found.") };

        var result = Result.Failure(errors);

        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.Errors.Count);
    }

    [Fact]
    public void Should_ImplicitlyConvertFromError_To_Result()
    {
        Result result = SampleError;

        Assert.False(result.IsSuccess);
        Assert.Equal("Test.Error", result.Errors[0].Code);
    }

    [Fact]
    public void Should_ReturnData_When_GenericResultIsSuccess()
    {
        var result = Result<string>.Success("hello");

        Assert.True(result.IsSuccess);
        Assert.Equal("hello", result.Data);
    }

    [Fact]
    public void Should_ThrowException_When_AccessingDataOnFailedResult()
    {
        var result = Result<string>.Failure(SampleError);

        Assert.Throws<InvalidOperationException>(() => _ = result.Data);
    }

    [Fact]
    public void Should_ImplicitlyConvertFromData_To_GenericResult()
    {
        Result<int> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Data);
    }

    [Fact]
    public void Should_ImplicitlyConvertFromError_To_GenericResult()
    {
        Result<int> result = SampleError;

        Assert.False(result.IsSuccess);
        Assert.Equal("Test.Error", result.Errors[0].Code);
    }

    [Fact]
    public void Should_BeFailure_When_GenericResultFailureWithMultipleErrors()
    {
        var errors = new[] { SampleError, Error.NotFound("Other.Error", "Not found.") };

        var result = Result<string>.Failure(errors);

        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.Errors.Count);
    }
}

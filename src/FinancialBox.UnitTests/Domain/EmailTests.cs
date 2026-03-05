using FinancialBox.Domain.Features.Users.Errors;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.UnitTests.Domain;

public class EmailTests
{
    [Fact]
    public void Should_CreateSuccessfully_When_EmailIsValid()
    {
        // Arrange
        const string emailAddress = "teste@gmail.com";

        // Act
        var result = Email.Create(emailAddress);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(emailAddress, result.Data.Address);
    }

    [Fact]
    public void Should_ReturnError_When_EmailIsEmpty()
    {
        // Arrange
        const string emailAddress = "";

        // Act
        var result = Email.Create(emailAddress);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
    }
}


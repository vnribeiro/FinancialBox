using FinancialBox.Domain.Features.Accounts.Errors;
using FinancialBox.Domain.Features.Accounts.ValueObjects;

namespace FinancialBox.UnitTests.Domain.Users;

public class EmailTests
{
    [Fact]
    public void Should_ReturnSuccess_When_AddressIsValid()
    {
        //Arrange & Act
        var result = Email.Create("test@example.com");

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("test@example.com", result.Data.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_ReturnError_When_AddressIsNullOrWhiteSpace(string? address)
    {
        //Arrange & Act
        var result = Email.Create(address!);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EmailErrors.Empty.Code, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("invalidemail")]
    [InlineData("test@")]
    [InlineData("test@domain")]
    [InlineData("@example.com")]
    public void Should_ReturnError_When_AddressHasInvalidFormat(string address)
    {
        //Arrange & Act
        var result = Email.Create(address);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EmailErrors.InvalidFormat.Code, result.Errors[0].Code);
    }
}
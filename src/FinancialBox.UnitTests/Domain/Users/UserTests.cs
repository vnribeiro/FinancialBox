using FinancialBox.Domain.Features.Users;

namespace FinancialBox.UnitTests.Domain.Users;

public class UserTests
{
    private static readonly Guid AccountId = Guid.NewGuid();

    [Fact]
    public void Should_CreateUser_When_AllFieldsAreValid()
    {
        var user = User.Create(AccountId, "John", "Doe");

        Assert.Equal(AccountId, user.AccountId);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
    }

    [Fact]
    public void Should_ReturnFullName_When_GetFullNameCalled()
    {
        var user = User.Create(AccountId, "Jane", "Smith");

        Assert.Equal("Jane Smith", user.GetFullName());
    }

    [Fact]
    public void Should_UpdateName_When_UpdateNameCalled()
    {
        var user = User.Create(AccountId, "Old", "Name");

        user.UpdateName("New", "Name");

        Assert.Equal("New", user.FirstName);
        Assert.Equal("Name", user.LastName);
    }
}

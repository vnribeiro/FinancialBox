using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.UnitTests.Domain.Users;

public class PasswordTests
{
    [Fact]
    public void Should_CreatePassword_When_ValidHashProvided()
    {
        var hash = "hashed_password_123";
        var password = Password.FromHash(hash);

        Assert.Equal(hash, password.Hash);
    }

    [Fact]
    public void Should_ReturnProtected_When_ToStringCalled()
    {
        var password = Password.FromHash("any_hash");

        Assert.Equal("[PROTECTED]", password.ToString());
    }

    [Fact]
    public void Should_BeEqual_When_HashesAreTheSame()
    {
        var hash = "same_hash";
        var p1 = Password.FromHash(hash);
        var p2 = Password.FromHash(hash);

        Assert.Equal(p1, p2);
        Assert.True(p1 == p2);
    }

    [Fact]
    public void Should_NotBeEqual_When_HashesAreDifferent()
    {
        var p1 = Password.FromHash("hash_one");
        var p2 = Password.FromHash("hash_two");

        Assert.NotEqual(p1, p2);
        Assert.True(p1 != p2);
    }

    [Fact]
    public void Should_NotBeEqual_When_OtherIsNull()
    {
        var password = Password.FromHash("some_hash");

        Assert.False(password.Equals(null));
        Assert.True(password != null);
    }

    [Fact]
    public void Should_BeEqual_When_ComparedAsObject()
    {
        var hash = "same_hash";
        var p1 = Password.FromHash(hash);
        var p2 = Password.FromHash(hash);

        Assert.True(p1.Equals((object)p2));
    }

    [Fact]
    public void Should_HaveSameHashCode_When_HashesAreEqual()
    {
        var p1 = Password.FromHash("my_hash");
        var p2 = Password.FromHash("my_hash");

        Assert.Equal(p1.GetHashCode(), p2.GetHashCode());
    }
}

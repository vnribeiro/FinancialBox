using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.UnitTests.Domain.Accounts;

public class EmailConfirmationTokenTests
{
    private static readonly Guid AccountId = Guid.NewGuid();

    private static EmailConfirmationToken CreateToken(DateTime expiresAt)
        => EmailConfirmationToken.Create(AccountId, "hashed_token", expiresAt);

    [Fact]
    public void Should_AllowValidation_When_TokenIsNotUsedAndNotExpired()
    {
        var token = CreateToken(DateTime.UtcNow.AddMinutes(30));
        Assert.True(token.CanValidate(DateTime.UtcNow));
    }

    [Fact]
    public void Should_DenyValidation_When_TokenIsExpired()
    {
        var token = CreateToken(DateTime.UtcNow.AddMinutes(-1));
        Assert.False(token.CanValidate(DateTime.UtcNow));
    }

    [Fact]
    public void Should_DenyValidation_When_TokenIsAlreadyUsed()
    {
        var token = CreateToken(DateTime.UtcNow.AddMinutes(30));
        token.MarkAsUsed(DateTime.UtcNow);
        Assert.False(token.CanValidate(DateTime.UtcNow));
    }

    [Fact]
    public void Should_SetUsedAt_When_MarkAsUsedCalled()
    {
        var token = CreateToken(DateTime.UtcNow.AddMinutes(30));
        var now = DateTime.UtcNow;
        token.MarkAsUsed(now);
        Assert.Equal(now, token.UsedAt);
    }
}

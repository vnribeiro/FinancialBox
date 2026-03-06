using FinancialBox.Domain.Features.Users;

namespace FinancialBox.UnitTests.Domain.Users;

public class EmailVerificationCodeTests
{
    private static readonly Guid UserId = Guid.NewGuid();
    private const int MaxAttempts = 3;

    private static EmailVerificationCode CreateCode(DateTime expiresAt)
        => EmailVerificationCode.Create(UserId, "user@example.com", "123456", "hashed_code", expiresAt);

    [Fact]
    public void Should_AllowValidation_When_CodeIsNotUsed_NotExpired_And_BelowMaxAttempts()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));

        Assert.True(code.CanValidate(DateTime.UtcNow, MaxAttempts));
    }

    [Fact]
    public void Should_DenyValidation_When_CodeIsExpired()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(-1));

        Assert.False(code.CanValidate(DateTime.UtcNow, MaxAttempts));
    }

    [Fact]
    public void Should_DenyValidation_When_CodeIsAlreadyUsed()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));
        code.MarkAsUsed(DateTime.UtcNow);

        Assert.False(code.CanValidate(DateTime.UtcNow, MaxAttempts));
    }

    [Fact]
    public void Should_DenyValidation_When_MaxAttemptsReached()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));
        code.RegisterFailedAttempt();
        code.RegisterFailedAttempt();
        code.RegisterFailedAttempt();

        Assert.False(code.CanValidate(DateTime.UtcNow, MaxAttempts));
    }

    [Fact]
    public void Should_IncrementAttempts_When_RegisterFailedAttemptCalled()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));

        code.RegisterFailedAttempt();
        code.RegisterFailedAttempt();

        Assert.Equal(2, code.Attempts);
    }

    [Fact]
    public void Should_SetUsedAt_When_MarkAsUsedCalled()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));
        var now = DateTime.UtcNow;

        code.MarkAsUsed(now);

        Assert.Equal(now, code.UsedAt);
    }

    [Fact]
    public void Should_RaiseDomainEvent_When_Created()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));

        Assert.Single(code.DomainEvents);
    }

    [Fact]
    public void Should_AllowValidation_When_AttemptsAreBelowMax()
    {
        var code = CreateCode(DateTime.UtcNow.AddMinutes(10));
        code.RegisterFailedAttempt();
        code.RegisterFailedAttempt();

        Assert.True(code.CanValidate(DateTime.UtcNow, MaxAttempts));
    }
}

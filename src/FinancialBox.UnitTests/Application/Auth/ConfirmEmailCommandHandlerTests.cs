using FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.UnitTests.Application.Fakes;

namespace FinancialBox.UnitTests.Application.Auth;

public class ConfirmEmailCommandHandlerTests
{
    private readonly FakeAccountRepository _accountRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ConfirmEmailCommandHandler _handler;

    public ConfirmEmailCommandHandlerTests()
    {
        _handler = new ConfirmEmailCommandHandler(_unitOfWork, _accountRepository);
    }

    private (Account account, string token) CreateAccountWithToken(bool expired = false, bool used = false)
    {
        var account = Account.Create(
            Email.Create("user@example.com").Data,
            Password.FromHash("hash"));

        var expiresAt = expired ? DateTime.UtcNow.AddMinutes(-1) : DateTime.UtcNow.AddMinutes(30);
        var token = EmailConfirmationToken.Create(account.Id, expiresAt);

        if (used) token.MarkAsUsed(DateTime.UtcNow.AddMinutes(-1));

        account.AddEmailConfirmationToken(token);
        _accountRepository.Seed(account);
        return (account, token.Token);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredToken_When_TokenNotFound()
    {
        var result = await _handler.Handle(new ConfirmEmailCommand("unknown-token"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidOrExpiredToken.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredToken_When_TokenIsExpired()
    {
        var (_, token) = CreateAccountWithToken(expired: true);

        var result = await _handler.Handle(new ConfirmEmailCommand(token), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidOrExpiredToken.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredToken_When_TokenAlreadyUsed()
    {
        var (_, token) = CreateAccountWithToken(used: true);

        var result = await _handler.Handle(new ConfirmEmailCommand(token), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidOrExpiredToken.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ConfirmEmail_When_TokenIsValid()
    {
        var (account, token) = CreateAccountWithToken();

        var result = await _handler.Handle(new ConfirmEmailCommand(token), default);

        Assert.True(result.IsSuccess);
        Assert.True(account.IsEmailConfirmed);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnSuccess_Without_Commit_When_EmailAlreadyConfirmed()
    {
        var (account, token) = CreateAccountWithToken();
        account.ConfirmEmail();

        var result = await _handler.Handle(new ConfirmEmailCommand(token), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_MarkTokenAsUsed_When_ConfirmationSucceeds()
    {
        var (account, token) = CreateAccountWithToken();

        await _handler.Handle(new ConfirmEmailCommand(token), default);

        var confirmationToken = account.EmailConfirmationTokens.First();
        Assert.NotNull(confirmationToken.UsedAt);
    }
}

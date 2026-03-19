using FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.UnitTests.Application.Fakes;

namespace FinancialBox.UnitTests.Application.Auth;

public class ConfirmEmailCommandHandlerTests
{
    private readonly FakeAccountRepository _accountRepository = new();
    private readonly FakeEmailConfirmationTokenRepository _tokenRepository = new();
    private readonly FakeHasherService _hasherService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ConfirmEmailCommandHandler _handler;

    public ConfirmEmailCommandHandlerTests()
    {
        _handler = new ConfirmEmailCommandHandler(
            _unitOfWork,
            _accountRepository,
            _tokenRepository,
            _hasherService);
    }

    private Account CreateAccount(string email = "user@example.com")
    {
        var account = Account.Create(
            Email.Create(email).Data,
            Password.FromHash("hash"));
        _accountRepository.Seed(account);
        return account;
    }

    private EmailConfirmationToken CreateValidToken(Guid accountId, string plainToken = "test-token")
    {
        var token = EmailConfirmationToken.Create(accountId, _hasherService.Hash(plainToken), DateTime.UtcNow.AddMinutes(30));
        _tokenRepository.Seed(token);
        return token;
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
        var account = CreateAccount();
        var expiredToken = EmailConfirmationToken.Create(account.Id, _hasherService.Hash("my-token"), DateTime.UtcNow.AddMinutes(-1));
        _tokenRepository.Seed(expiredToken);

        var result = await _handler.Handle(new ConfirmEmailCommand("my-token"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidOrExpiredToken.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredToken_When_TokenAlreadyUsed()
    {
        var account = CreateAccount();
        var token = CreateValidToken(account.Id, "valid-token");
        token.MarkAsUsed(DateTime.UtcNow.AddMinutes(-1));

        var result = await _handler.Handle(new ConfirmEmailCommand("valid-token"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidOrExpiredToken.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ConfirmEmail_When_TokenIsValid()
    {
        var account = CreateAccount();
        CreateValidToken(account.Id, "valid-token");

        var result = await _handler.Handle(new ConfirmEmailCommand("valid-token"), default);

        Assert.True(result.IsSuccess);
        Assert.True(account.IsEmailConfirmed);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnSuccess_Without_Commit_When_EmailAlreadyConfirmed()
    {
        var account = CreateAccount();
        account.ConfirmEmail();
        CreateValidToken(account.Id, "valid-token");

        var result = await _handler.Handle(new ConfirmEmailCommand("valid-token"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_MarkTokenAsUsed_When_ConfirmationSucceeds()
    {
        var account = CreateAccount();
        var token = CreateValidToken(account.Id, "valid-token");

        await _handler.Handle(new ConfirmEmailCommand("valid-token"), default);

        Assert.NotNull(token.UsedAt);
    }
}

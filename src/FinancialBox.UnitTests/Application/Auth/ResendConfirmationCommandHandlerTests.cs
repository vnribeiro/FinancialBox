using FinancialBox.Application.Features.Auth;
using FinancialBox.Application.Features.Auth.Commands.ResendConfirmation;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.UnitTests.Application.Fakes;
using Microsoft.Extensions.Options;

namespace FinancialBox.UnitTests.Application.Auth;

public class ResendConfirmationCommandHandlerTests
{
    private readonly FakeAccountRepository _accountRepository = new();
    private readonly FakeEmailService _emailService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ResendConfirmationCommandHandler _handler;

    private const int CooldownSeconds = 60;
    private const int MaxSendsPerHour = 5;

    public ResendConfirmationCommandHandlerTests()
    {
        var options = Options.Create(new AuthOptions
        {
            EmailConfirmation = new AuthOptions.EmailConfirmationSettings
            {
                CooldownSeconds = CooldownSeconds,
                MaxSendsPerHour = MaxSendsPerHour,
                ExpirationMinutes = 30
            }
        });

        _handler = new ResendConfirmationCommandHandler(
            _unitOfWork, _accountRepository, _emailService, options);
    }

    private Account CreateUnconfirmedAccount(string email = "user@example.com")
    {
        var account = Account.Create(Email.Create(email).Data, Password.FromHash("hash"));
        _accountRepository.Seed(account);
        return account;
    }

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var result = await _handler.Handle(new ResendConfirmationCommand("bad-email"), default);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnSuccess_Silently_When_AccountNotFound()
    {
        var result = await _handler.Handle(new ResendConfirmationCommand("ghost@example.com"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnSuccess_Silently_When_EmailAlreadyConfirmed()
    {
        var account = CreateUnconfirmedAccount();
        account.ConfirmEmail();

        var result = await _handler.Handle(new ResendConfirmationCommand("user@example.com"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnResendLimitReached_When_CooldownNotExpired()
    {
        var account = CreateUnconfirmedAccount();
        account.AddEmailConfirmationToken(
            EmailConfirmationToken.Create(account.Id, DateTime.UtcNow.AddMinutes(30)));

        var result = await _handler.Handle(new ResendConfirmationCommand("user@example.com"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.ResendLimitReached.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnResendLimitReached_When_MaxSendsPerHourReached()
    {
        var account = CreateUnconfirmedAccount();

        for (var i = 0; i < MaxSendsPerHour; i++)
            account.AddEmailConfirmationToken(
                EmailConfirmationToken.Create(account.Id, DateTime.UtcNow.AddMinutes(-30 + i)));

        var result = await _handler.Handle(new ResendConfirmationCommand("user@example.com"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.ResendLimitReached.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnSuccess_And_CreateNewToken_When_NoRecentTokenExists()
    {
        CreateUnconfirmedAccount();

        var result = await _handler.Handle(new ResendConfirmationCommand("user@example.com"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_SendConfirmationLink_When_NoRecentTokenExists()
    {
        CreateUnconfirmedAccount();

        await _handler.Handle(new ResendConfirmationCommand("user@example.com"), default);

        Assert.Single(_emailService.ConfirmationLinksSent);
        Assert.Equal("user@example.com", _emailService.ConfirmationLinksSent[0].To);
    }
}

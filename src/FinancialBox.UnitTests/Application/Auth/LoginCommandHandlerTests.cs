using FinancialBox.Application.Features.Auth;
using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.UnitTests.Application.Fakes;
using Microsoft.Extensions.Options;

namespace FinancialBox.UnitTests.Application.Auth;

public class LoginCommandHandlerTests
{
    private readonly FakeAccountRepository _accountRepository = new();
    private readonly FakeHasherService _hasherService = new();
    private readonly FakeJwtService _jwtService = new();
    private readonly FakeTokenGeneratorService _tokenGeneratorService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        var options = Options.Create(new AuthOptions
        {
            RefreshToken = new AuthOptions.RefreshTokenSettings { ExpirationDays = 7 }
        });

        _handler = new LoginCommandHandler(
            _unitOfWork,
            _accountRepository,
            _jwtService,
            _hasherService,
            _tokenGeneratorService,
            options);
    }

    private Account CreateConfirmedAccount(string email = "user@example.com", string password = "secret")
    {
        var account = Account.Create(
            Email.Create(email).Data,
            Password.FromHash(_hasherService.Hash(password)));
        account.ConfirmEmail();
        _accountRepository.Seed(account);
        return account;
    }

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var result = await _handler.Handle(new LoginCommand("not-an-email", "password"), default);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnInvalidCredentials_When_UserNotFound()
    {
        var result = await _handler.Handle(new LoginCommand("unknown@example.com", "password"), default);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidCredentials.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidCredentials_When_PasswordIsWrong()
    {
        CreateConfirmedAccount(password: "correct");
        var result = await _handler.Handle(new LoginCommand("user@example.com", "wrong"), default);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidCredentials.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnEmailNotConfirmed_When_EmailIsNotConfirmed()
    {
        var account = Account.Create(
            Email.Create("user@example.com").Data,
            Password.FromHash(_hasherService.Hash("secret")));
        _accountRepository.Seed(account);

        var result = await _handler.Handle(new LoginCommand("user@example.com", "secret"), default);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.EmailNotConfirmed.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnToken_When_CredentialsAreValid()
    {
        CreateConfirmedAccount(password: "secret");
        var result = await _handler.Handle(new LoginCommand("user@example.com", "secret"), default);
        Assert.True(result.IsSuccess);
        Assert.Equal("fake_token", result.Data.AccessToken);
    }
}

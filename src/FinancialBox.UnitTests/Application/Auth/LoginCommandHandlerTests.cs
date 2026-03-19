using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.Domain.Features.Users;
using FinancialBox.UnitTests.Application.Fakes;

namespace FinancialBox.UnitTests.Application.Auth;

public class LoginCommandHandlerTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeSecureHashService _hashService = new();
    private readonly FakeJwtService _jwtService = new();
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _handler = new LoginCommandHandler(_userRepository, _hashService, _jwtService);
    }

    private User CreateConfirmedUser(string email = "user@example.com", string password = "secret")
    {
        var emailVo = Email.Create(email).Data;
        var passwordVo = Password.FromHash(_hashService.Hash(password));
        var user = User.Create("John", "Doe", emailVo, passwordVo);
        user.ConfirmEmail();
        return user;
    }

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var command = new LoginCommand("not-an-email", "password");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnInvalidCredentials_When_UserNotFound()
    {
        var command = new LoginCommand("unknown@example.com", "password");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidCredentials.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidCredentials_When_PasswordIsWrong()
    {
        var user = CreateConfirmedUser(password: "correct");
        _userRepository.Seed(user);

        var command = new LoginCommand("user@example.com", "wrong");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.InvalidCredentials.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnEmailNotConfirmed_When_EmailIsNotConfirmed()
    {
        var emailVo = Email.Create("user@example.com").Data;
        var user = User.Create("John", "Doe", emailVo, Password.FromHash("secret"));
        _userRepository.Seed(user);

        var command = new LoginCommand("user@example.com", "secret");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.EmailNotConfirmed.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnToken_When_CredentialsAreValid()
    {
        var user = CreateConfirmedUser(password: "secret");
        _userRepository.Seed(user);

        var command = new LoginCommand("user@example.com", "secret");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal("fake_token", result.Data.AccessToken);
    }
}
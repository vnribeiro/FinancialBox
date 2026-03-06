using FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;
using FinancialBox.Application.Options;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.ValueObjects;
using FinancialBox.UnitTests.Application.Fakes;
using Microsoft.Extensions.Options;

namespace FinancialBox.UnitTests.Application.Auth;

public class ConfirmEmailCommandHandlerTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeEmailVerificationCodeRepository _codeRepository = new();
    private readonly FakeSecureHashService _hashService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ConfirmEmailCommandHandler _handler;

    private const int MaxAttempts = 5;

    public ConfirmEmailCommandHandlerTests()
    {
        var options = Options.Create(new EmailVerificationOptions
        {
            MaxAttempts = MaxAttempts,
            CodeExpirationMinutes = 15
        });

        _handler = new ConfirmEmailCommandHandler(
            _unitOfWork,
            _userRepository,
            _codeRepository,
            _hashService,
            options);
    }

    private User CreateUnconfirmedUser(string email = "user@example.com")
    {
        var user = User.Create("John", "Doe",
            Email.Create(email).Data,
            Password.FromHash("hash"));
        _userRepository.Seed(user);
        return user;
    }

    private EmailVerificationCode CreateValidCode(Guid userId, string plainCode = "123456")
    {
        var code = EmailVerificationCode.Create(
            userId,
            "user@example.com",
            plainCode,
            _hashService.Hash(plainCode),
            DateTime.UtcNow.AddMinutes(15));
        _codeRepository.Seed(code);
        return code;
    }

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var command = new ConfirmEmailCommand("bad-email", "123456");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredCode_When_UserNotFound()
    {
        var command = new ConfirmEmailCommand("ghost@example.com", "123456");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal("Auth.InvalidOrExpiredCode", result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnSuccess_When_EmailAlreadyConfirmed()
    {
        var user = CreateUnconfirmedUser();
        user.ConfirmEmail();

        var command = new ConfirmEmailCommand("user@example.com", "123456");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredCode_When_NoVerificationCodeExists()
    {
        CreateUnconfirmedUser();

        var command = new ConfirmEmailCommand("user@example.com", "123456");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal("Auth.InvalidOrExpiredCode", result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredCode_When_CodeIsExpired()
    {
        var user = CreateUnconfirmedUser();
        var expiredCode = EmailVerificationCode.Create(
            user.Id, "user@example.com", "123456", _hashService.Hash("123456"),
            DateTime.UtcNow.AddMinutes(-1)); // already expired
        _codeRepository.Seed(expiredCode);

        var command = new ConfirmEmailCommand("user@example.com", "123456");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal("Auth.InvalidOrExpiredCode", result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnInvalidOrExpiredCode_And_IncrementAttempts_When_CodeIsWrong()
    {
        var user = CreateUnconfirmedUser();
        var code = CreateValidCode(user.Id, plainCode: "123456");

        var command = new ConfirmEmailCommand("user@example.com", "999999");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal("Auth.InvalidOrExpiredCode", result.Errors[0].Code);
        Assert.Equal(1, code.Attempts);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ConfirmEmail_When_CodeIsCorrect()
    {
        var user = CreateUnconfirmedUser();
        CreateValidCode(user.Id, plainCode: "123456");

        var command = new ConfirmEmailCommand("user@example.com", "123456");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.True(user.IsEmailConfirmed);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }
}

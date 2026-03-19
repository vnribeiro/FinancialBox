using FinancialBox.Application.Features.Auth;
using FinancialBox.Application.Features.Auth.Commands.ResendConfirmation;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.ValueObjects;
using FinancialBox.UnitTests.Application.Fakes;
using Microsoft.Extensions.Options;

namespace FinancialBox.UnitTests.Application.Auth;

public class ResendConfirmationCommandHandlerTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeEmailVerificationCodeRepository _codeRepository = new();
    private readonly FakeSecureHashService _hashService = new();
    private readonly FakeEmailService _emailService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ResendConfirmationCommandHandler _handler;

    private const int CooldownSeconds = 60;
    private const int MaxSendsPerHour = 5;

    public ResendConfirmationCommandHandlerTests()
    {
        var options = Options.Create(new EmailVerificationOptions
        {
            CooldownSeconds = CooldownSeconds,
            MaxSendsPerHour = MaxSendsPerHour,
            CodeExpirationMinutes = 15
        });

        _handler = new ResendConfirmationCommandHandler(
            _unitOfWork,
            _userRepository,
            _codeRepository,
            _hashService,
            _emailService,
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

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var command = new ResendConfirmationCommand("bad-email");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnSuccess_Silently_When_UserNotFound()
    {
        var command = new ResendConfirmationCommand("ghost@example.com");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnSuccess_Silently_When_EmailAlreadyConfirmed()
    {
        var user = CreateUnconfirmedUser();
        user.ConfirmEmail();

        var command = new ResendConfirmationCommand("user@example.com");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_ReturnResendLimitReached_When_CooldownNotExpired()
    {
        var user = CreateUnconfirmedUser();

        var recentCode = EmailVerificationCode.Create(
            user.Id, "hash",
            DateTime.UtcNow.AddMinutes(15));
        _codeRepository.Seed(recentCode);

        var command = new ResendConfirmationCommand("user@example.com");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.ResendLimitReached.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnResendLimitReached_When_MaxSendsPerHourReached()
    {
        var user = CreateUnconfirmedUser();

        for (int i = 0; i < MaxSendsPerHour; i++)
        {
            var code = EmailVerificationCode.Create(
                user.Id, $"hash{i}",
                DateTime.UtcNow.AddMinutes(15));
            _codeRepository.Seed(code);
        }

        var repoWithNullLatest = new FakeEmailVerificationCodeRepositoryWithNullLatest(_codeRepository);
        var options = Options.Create(new EmailVerificationOptions
        {
            CooldownSeconds = CooldownSeconds,
            MaxSendsPerHour = MaxSendsPerHour,
            CodeExpirationMinutes = 15
        });

        var handler = new ResendConfirmationCommandHandler(
            _unitOfWork, _userRepository, repoWithNullLatest, _hashService, _emailService, options);

        var command = new ResendConfirmationCommand("user@example.com");

        var result = await handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthErrors.ResendLimitReached.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnSuccess_And_CreateNewCode_When_NoRecentCodeExists()
    {
        CreateUnconfirmedUser();

        var command = new ResendConfirmationCommand("user@example.com");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_SendVerificationEmail_When_NoRecentCodeExists()
    {
        CreateUnconfirmedUser();

        await _handler.Handle(new ResendConfirmationCommand("user@example.com"), default);

        Assert.Single(_emailService.VerificationCodesSent);
        Assert.Equal("user@example.com", _emailService.VerificationCodesSent[0].To);
    }

    private sealed class FakeEmailVerificationCodeRepositoryWithNullLatest(
        FakeEmailVerificationCodeRepository inner)
        : FakeEmailVerificationCodeRepository
    {
        public override Task<EmailVerificationCode?> GetMostRecentByUserIdAsync(
            Guid userId, CancellationToken cancellationToken = default)
            => Task.FromResult<EmailVerificationCode?>(null);

        public override Task<int> CountSentByUserIdAfterAsync(
            Guid userId, DateTime after, CancellationToken cancellationToken = default)
            => inner.CountSentByUserIdAfterAsync(userId, after, cancellationToken);
    }
}

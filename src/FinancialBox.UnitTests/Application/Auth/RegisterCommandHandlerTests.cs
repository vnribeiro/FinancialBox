using FinancialBox.Application.Features.Auth;
using FinancialBox.Application.Features.Auth.Commands.Register;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.Errors;
using FinancialBox.UnitTests.Application.Fakes;
using Microsoft.Extensions.Options;

namespace FinancialBox.UnitTests.Application.Auth;

public class RegisterCommandHandlerTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeRoleRepository _roleRepository = new();
    private readonly FakeEmailVerificationCodeRepository _codeRepository = new();
    private readonly FakeSecureHashService _hashService = new();
    private readonly FakeEmailService _emailService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        var options = Options.Create(new OtpOptions
        {
            CodeExpirationMinutes = 15
        });

        _handler = new RegisterCommandHandler(
            _unitOfWork,
            _userRepository,
            _roleRepository,
            _codeRepository,
            _hashService,
            _emailService,
            options);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var command = new RegisterCommand("John", "Doe", "bad-email", "Password1!");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnEmailAlreadyExists_When_EmailIsTaken()
    {
        var existing = User.Create("Jane", "Doe",
            Email.Create("taken@example.com").Data,
            Password.FromHash("hash"));
        _userRepository.Seed(existing);

        var command = new RegisterCommand("John", "Doe", "taken@example.com", "Password1!");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.EmailAlreadyInUse.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnSuccess_And_CreateUser_When_DataIsValid()
    {
        var command = new RegisterCommand("John", "Doe", "new@example.com", "Password1!");

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal("new@example.com", result.Data.Email);
        Assert.NotEqual(Guid.Empty, result.Data.Id);
    }

    [Fact]
    public async Task Should_CommitOnce_When_RegistrationSucceeds()
    {
        var command = new RegisterCommand("John", "Doe", "new@example.com", "Password1!");

        await _handler.Handle(command, default);

        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_CreateVerificationCode_When_RegistrationSucceeds()
    {
        var command = new RegisterCommand("John", "Doe", "new@example.com", "Password1!");

        await _handler.Handle(command, default);

        var codes = await _codeRepository.GetAllAsync();
        Assert.Single(codes);
    }

    [Fact]
    public async Task Should_SendVerificationEmail_When_RegistrationSucceeds()
    {
        var command = new RegisterCommand("John", "Doe", "new@example.com", "Password1!");

        await _handler.Handle(command, default);

        Assert.Single(_emailService.VerificationCodesSent);
        Assert.Equal("new@example.com", _emailService.VerificationCodesSent[0].To);
    }
}

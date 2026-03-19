using FinancialBox.Application.Features.Auth;
using FinancialBox.Application.Features.Auth.Commands.Register;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.Domain.Features.Users.Errors;
using FinancialBox.UnitTests.Application.Fakes;
using Microsoft.Extensions.Options;

namespace FinancialBox.UnitTests.Application.Auth;

public class RegisterCommandHandlerTests
{
    private readonly FakeAccountRepository _accountRepository = new();
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeRoleRepository _roleRepository = new();
    private readonly FakeHasherService _hasherService = new();
    private readonly FakeEmailService _emailService = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        var options = Options.Create(new AuthOptions
        {
            EmailConfirmation = new AuthOptions.EmailConfirmationSettings { ExpirationMinutes = 30 }
        });

        _handler = new RegisterCommandHandler(
            _unitOfWork,
            _accountRepository,
            _userRepository,
            _roleRepository,
            _hasherService,
            _emailService,
            options);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_EmailIsInvalid()
    {
        var result = await _handler.Handle(new RegisterCommand("John", "Doe", "bad-email", "Password1!"), default);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_ReturnEmailAlreadyExists_When_EmailIsTaken()
    {
        var existing = Account.Create(Email.Create("taken@example.com").Data, Password.FromHash("hash"));
        _accountRepository.Seed(existing);

        var result = await _handler.Handle(new RegisterCommand("John", "Doe", "taken@example.com", "Password1!"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.EmailAlreadyInUse.Code, result.Errors[0].Code);
    }

    [Fact]
    public async Task Should_ReturnSuccess_When_DataIsValid()
    {
        var result = await _handler.Handle(new RegisterCommand("John", "Doe", "new@example.com", "Password1!"), default);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data.Id);
    }

    [Fact]
    public async Task Should_CommitOnce_When_RegistrationSucceeds()
    {
        await _handler.Handle(new RegisterCommand("John", "Doe", "new@example.com", "Password1!"), default);
        Assert.Equal(1, _unitOfWork.CommitCount);
    }

    [Fact]
    public async Task Should_CreateConfirmationToken_When_RegistrationSucceeds()
    {
        await _handler.Handle(new RegisterCommand("John", "Doe", "new@example.com", "Password1!"), default);

        var account = _accountRepository.All().First(a => a.Email.Address == "new@example.com");
        Assert.Single(account.EmailConfirmationTokens);
    }

    [Fact]
    public async Task Should_SendConfirmationLink_When_RegistrationSucceeds()
    {
        await _handler.Handle(new RegisterCommand("John", "Doe", "new@example.com", "Password1!"), default);

        Assert.Single(_emailService.ConfirmationLinksSent);
        Assert.Equal("new@example.com", _emailService.ConfirmationLinksSent[0].To);
    }
}

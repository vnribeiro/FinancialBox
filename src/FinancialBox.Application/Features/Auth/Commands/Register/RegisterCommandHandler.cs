using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.Errors;
using FinancialBox.Domain.Primitives;
using Microsoft.Extensions.Options;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IHasherService hasherService,
    IEmailService emailService,
    IOptions<AuthOptions> options)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly AuthOptions _authOptions = options.Value;

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result<RegisterResponse>.Failure(emailResult.Errors);

        if (await accountRepository.EmailExistsAsync(emailResult.Data.Address, cancellationToken))
            return UserErrors.EmailAlreadyInUse;

        var password = Password.FromHash(hasherService.Hash(request.Password));
        var account = Account.Create(emailResult.Data, password);

        var role = await roleRepository.GetByNameAsync(Role.DefaultName, cancellationToken);
        account.AddRole(role!);

        var plainToken = Guid.NewGuid().ToString();
        var tokenHash = hasherService.Hash(plainToken);
        var expiresAt = DateTime.UtcNow.AddMinutes(_authOptions.EmailConfirmation.ExpirationMinutes);
        account.AddEmailConfirmationToken(EmailConfirmationToken.Create(account.Id, tokenHash, expiresAt));

        var user = User.Create(account.Id, request.FirstName, request.LastName);

        await accountRepository.AddAsync(account, cancellationToken);
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        await emailService.SendConfirmationLinkAsync(account.Email.Address, plainToken, cancellationToken);

        return Result<RegisterResponse>.Success(new RegisterResponse(account.Id));
    }
}

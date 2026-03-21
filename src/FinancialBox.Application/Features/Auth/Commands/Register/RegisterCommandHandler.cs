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

        var expiresAt = DateTime.UtcNow.AddMinutes(_authOptions.EmailConfirmation.ExpirationMinutes);
        var password = Password.FromHash(hasherService.Hash(request.Password));
        var account = Account.Register(emailResult.Data, password, expiresAt);

        var role = await roleRepository.GetByNameAsync(Role.DefaultName, cancellationToken);
        account.AddRole(role!);

        var user = User.Create(account.Id, request.FirstName, request.LastName);

        await accountRepository.AddAsync(account, cancellationToken);
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<RegisterResponse>.Success(new RegisterResponse(account.Id));
    }
}

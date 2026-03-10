using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Application.Options;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.Errors;
using FinancialBox.Domain.Features.Users.ValueObjects;
using FinancialBox.Domain.Primitives;
using Microsoft.Extensions.Options;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IEmailVerificationCodeRepository emailVerificationCodeRepository,
    ISecureHashService secureHashService,
    IOptions<EmailVerificationOptions> emailVerificationOptions)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly EmailVerificationOptions _emailVerificationOptions = emailVerificationOptions.Value;

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result<RegisterResponse>.Failure(emailResult.Errors);

        if (await userRepository.EmailExistsAsync(emailResult.Data.Address, cancellationToken))
            return UserErrors.EmailAlreadyInUse;

        var password = Password.FromHash(secureHashService.Hash(request.Password));

        var role = await roleRepository.GetByNameAsync(Role.DefaultName, cancellationToken);
        var user = User.Create(request.FirstName, request.LastName, emailResult.Data, password);
        user.AddRole(role!);

        await userRepository.AddAsync(user, cancellationToken);

        var plainCode = OtpGenerator.Generate();
        var codeHash = secureHashService.Hash(plainCode);
        var expiresAt = DateTime.UtcNow.AddMinutes(_emailVerificationOptions.CodeExpirationMinutes);

        var emailVerificationCode = EmailVerificationCode.Create(user.Id, user.Email.Address, plainCode, codeHash, expiresAt);
        await emailVerificationCodeRepository.AddAsync(emailVerificationCode, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return Result<RegisterResponse>.Success(new RegisterResponse(user.Id, user.Email.Address));
    }
}



using System.Security.Cryptography;
using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IEmailVerificationRepository emailVerificationCodeRepository,
    ISecureHashService secretHasherService)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);

        if (await userRepository.EmailExistsAsync(email.Address, cancellationToken))
            return Result<RegisterResponse>.Failure(Error.ResourceConflict("Email already exists."));

        var passwordHash = secretHasherService.Hash(request.Password);
        var password = Password.FromHash(passwordHash);

        var role = await roleRepository.GetByNameAsync(Role.DefaultName, cancellationToken);
        var user = User.Register(request.FirstName, request.LastName, email, password);
        user.AddRole(role!);

        await userRepository.AddAsync(user, cancellationToken);

        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        var otpHash = secretHasherService.Hash(otp);

        await emailVerificationCodeRepository.AddAsync(new EmailVerification(user.Id, otpHash, DateTime.UtcNow.AddMinutes(15)), cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return Result<RegisterResponse>.Success(new RegisterResponse(user.Id, user.Email.Address));
    }
}

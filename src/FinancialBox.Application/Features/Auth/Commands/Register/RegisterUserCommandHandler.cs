using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Domain.Features.User;
using FinancialBox.Domain.Features.User.ValueObjects;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasherService passwordHasher)
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);

        if (await userRepository.EmailExistsAsync(email.Address, cancellationToken))
        {
            return Result<RegisterUserResponse>.Failure(Error.ResourceConflict("Email already exists."));
        }

        var passwordHash = passwordHasher.Hash(request.Password);
        var password = Password.FromHash(passwordHash);

        var userRole = await roleRepository.GetByNameAsync(Role.DefaultName, cancellationToken);

        if (userRole is null)
        {
            return Result<RegisterUserResponse>.Failure(Error.ResourceNotFound("Role 'User' not found."));
        }

        var user = User.Register(request.FirstName, request.LastName, email, password);
        user.AddRole(userRole);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result<RegisterUserResponse>.Success(new RegisterUserResponse(user.Id, user.Email.Address));
    }
}

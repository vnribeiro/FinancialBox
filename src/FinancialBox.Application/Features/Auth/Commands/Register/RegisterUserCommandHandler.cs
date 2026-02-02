using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var password = Password.FromHash(request.Password);

        var user = User.Register(request.FirstName, request.LastName, email, password);
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result<RegisterUserResponse>.Success(new RegisterUserResponse(user.Id, user.Email.Address));
    }
}

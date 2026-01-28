using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Domain.Users;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Register(request.FirstName, request.LastName, request.Email, request.PasswordHash);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return Result<RegisterUserResponse>.Success(new RegisterUserResponse(user.Id, user.Email));
    }
}

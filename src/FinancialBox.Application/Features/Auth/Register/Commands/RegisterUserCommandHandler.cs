using FinancialBox.Application.Features.Auth.Register.Responses;
using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Persistence;
using FinancialBox.Domain.Users;

namespace FinancialBox.Application.Features.Auth.Register.Commands;

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

using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.Domain.Users;

namespace FinancialBox.Application.Auth.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{

    public LoginUserCommandHandler() {}

    public  Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.FirstName, request.LastName, request.Email, request.PasswordHash);

        var response = new LoginUserResponse(request.FirstName, request.LastName, request.Email, request.PasswordHash);

        return Task.FromResult(Result<LoginUserResponse>.Success(response));
    }
}
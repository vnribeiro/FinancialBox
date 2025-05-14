using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Commands.Auth.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public LoginUserCommandHandler()
    {

    }

    public Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginUserResponse(request.Name);
        return Task.FromResult(Result<LoginUserResponse>.Success(response));
    }
}

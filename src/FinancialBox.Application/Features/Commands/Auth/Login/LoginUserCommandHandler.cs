using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;
using FinancialBox.Domain.Entities;
using Mapster;

namespace FinancialBox.Application.Features.Commands.Auth.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public LoginUserCommandHandler() {}

    public Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();
        var response = user.Adapt<LoginUserResponse>();
        return Task.FromResult(Result<LoginUserResponse>.Success(response));
    }
}

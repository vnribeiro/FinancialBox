using FinancialBox.Application.Features.Auth.Login.Responses;
using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.Domain.Users;
using Mapster;

namespace FinancialBox.Application.Features.Auth.Login.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    public LoginUserCommandHandler() {}

    public Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();
        var response = user.Adapt<LoginUserResponse>();
        return Task.FromResult(Result<LoginUserResponse>.Success(response));
    }
}

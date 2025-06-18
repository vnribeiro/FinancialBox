using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.Domain.Users;
using Mapster;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
{
    public RegisterUserCommandHandler() {}

    public Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();
        var response = user.Adapt<RegisterUserResponse>();
        return Task.FromResult(Result.Success());
    }
}

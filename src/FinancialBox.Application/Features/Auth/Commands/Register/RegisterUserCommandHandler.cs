using System.ComponentModel;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;
using FinancialBox.Domain.Entities;
using Mapster;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public RegisterUserCommandHandler() {}

    public Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();
        var response = user.Adapt<RegisterUserResponse>();
        return Task.FromResult(Result<RegisterUserResponse>.Failure(Error.NotFound("wdw")));
    }
}

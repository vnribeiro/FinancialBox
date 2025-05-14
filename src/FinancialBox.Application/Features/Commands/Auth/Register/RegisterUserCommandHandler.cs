using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Commands.Auth.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public RegisterUserCommandHandler()
    {

    }

    public Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var response = new RegisterUserResponse(request.Name);
        return Task.FromResult(Result<RegisterUserResponse>.Success(response));
    }
}

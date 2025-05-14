using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserDto>
    {
        public RegisterUserCommandHandler()
        {

        }

        public Task<Result<RegisterUserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = new RegisterUserDto { Name = request.Name };
            return Task.FromResult(Result<RegisterUserDto>.Success(dto));
        }
    }
}

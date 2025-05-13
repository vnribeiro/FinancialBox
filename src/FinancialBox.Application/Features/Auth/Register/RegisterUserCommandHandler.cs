using FinancialBox.Shared.Contracts.Mediator;
using FinancialBox.Shared.ResultObjects;

namespace FinancialBox.Application.Features.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserDto>>
    {
        public RegisterUserCommandHandler()
        {

        }

        public Task<Result<RegisterUserDto>> Handle(RegisterUserCommand request)
        {
            var dto = new RegisterUserDto { Name = request.Name };
            return Task.FromResult(Result<RegisterUserDto>.Success(dto));
        }
    }
}

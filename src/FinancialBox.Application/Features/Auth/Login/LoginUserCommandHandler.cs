using FinancialBox.Shared.Contracts.Mediator;
using FinancialBox.Shared.ResultObjects;

namespace FinancialBox.Application.Features.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserDto>>
    {
        public LoginUserCommandHandler()
        {
            
        }

        public Task<Result<LoginUserDto>> Handle(LoginUserCommand request)
        {
            var dto = new LoginUserDto { Name = request.Name };
            return Task.FromResult(Result<LoginUserDto>.Success(dto));
        }
    }
}

using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserDto>
    {
        public LoginUserCommandHandler()
        {
            
        }

        public Task<Result<LoginUserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dto = new LoginUserDto { Name = request.Name };
            return Task.FromResult(Result<LoginUserDto>.Success(dto));
        }
    }
}

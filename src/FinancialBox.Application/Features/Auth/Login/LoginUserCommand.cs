using FinancialBox.Shared.Contracts.Mediator;
using FinancialBox.Shared.ResultObjects;

namespace FinancialBox.Application.Features.Auth.Login
{
    public class LoginUserCommand : IRequest<Result<LoginUserDto>>
    {
        public string Name { get; set; } = string.Empty;
    }
}

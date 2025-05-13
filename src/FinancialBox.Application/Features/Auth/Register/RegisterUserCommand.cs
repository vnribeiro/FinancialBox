using FinancialBox.Shared.Contracts.Mediator;
using FinancialBox.Shared.ResultObjects;

namespace FinancialBox.Application.Features.Auth.Register
{
    public class RegisterUserCommand : IRequest<Result<RegisterUserDto>>
    {
        public string Name { get; set; } = string.Empty;
    }
}

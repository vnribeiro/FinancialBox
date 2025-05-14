using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Auth.Login
{
    public class LoginUserCommand : ICommand<LoginUserDto>
    {
        public string Name { get; set; } = string.Empty;
    }
}

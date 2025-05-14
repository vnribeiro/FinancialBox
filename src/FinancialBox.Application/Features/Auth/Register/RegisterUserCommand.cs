using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Auth.Register
{
    public class RegisterUserCommand : ICommand<RegisterUserDto>
    {
        public string Name { get; set; } = string.Empty;
    }
}

using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Commands.Auth.Login;

public record LoginUserCommand(string Name) : ICommand<LoginUserResponse>;


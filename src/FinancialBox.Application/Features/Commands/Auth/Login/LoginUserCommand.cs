using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Commands.Auth.Login;

public sealed record LoginUserCommand(string Name) : ICommand<LoginUserResponse>;


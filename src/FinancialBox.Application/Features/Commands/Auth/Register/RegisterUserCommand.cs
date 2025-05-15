using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Commands.Auth.Register;

public sealed record RegisterUserCommand(string Name) : ICommand<RegisterUserResponse>;


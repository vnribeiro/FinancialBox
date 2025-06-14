using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginUserCommand(string Name) : IRequest<Result<LoginUserResponse>>;


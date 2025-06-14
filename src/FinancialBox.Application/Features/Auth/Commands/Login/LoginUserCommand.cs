using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginUserCommand(string FirstName, string LastName, string Email, string PasswordHash) : IRequest<Result<LoginUserResponse>>;
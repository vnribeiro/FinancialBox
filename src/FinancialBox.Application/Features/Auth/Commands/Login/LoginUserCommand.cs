using FinancialBox.Application.Features.Auth.Comands.Login.Responses;
using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginUserCommand(string FirstName, string LastName, string Email, string PasswordHash) : IRequest<Result<LoginUserResponse>>;
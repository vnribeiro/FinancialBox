using FinancialBox.Application.Features.Auth.Register.Responses;
using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Auth.Register.Commands;

public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string PasswordHash) : IRequest<Result<RegisterUserResponse>>;


using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string PasswordHash) : IRequest<Result>;


using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string PasswordHash) : IRequest<Result>;


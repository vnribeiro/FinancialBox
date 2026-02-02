using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<RegisterUserResponse>>;

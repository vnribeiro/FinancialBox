using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<RegisterResponse>>;

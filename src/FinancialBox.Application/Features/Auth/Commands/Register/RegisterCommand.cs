using FinancialBox.Domain.Primitives;
using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<RegisterResponse>>;

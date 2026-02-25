using FinancialBox.Domain.Primitives;
using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

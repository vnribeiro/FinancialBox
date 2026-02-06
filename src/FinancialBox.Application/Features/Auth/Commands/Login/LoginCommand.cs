using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

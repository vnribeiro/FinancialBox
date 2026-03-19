using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string Email, string Code) : IRequest<Result>;

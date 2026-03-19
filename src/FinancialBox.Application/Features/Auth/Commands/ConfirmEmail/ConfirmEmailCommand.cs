using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string Token) : IRequest<Result>;

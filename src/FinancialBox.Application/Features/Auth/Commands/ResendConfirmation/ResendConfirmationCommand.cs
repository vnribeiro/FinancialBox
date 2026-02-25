using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Commands.ResendConfirmation;

public sealed record ResendConfirmationCommand(string Email) : IRequest<Result>;

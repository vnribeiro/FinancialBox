using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Features.Auth.Commands.SignUp;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<RegisterResponse>>;

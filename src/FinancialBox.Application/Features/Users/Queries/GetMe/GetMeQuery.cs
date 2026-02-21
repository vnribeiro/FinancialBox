using FinancialBox.Application.Common;
using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Features.Users.Queries.GetMe;

public sealed record GetMeQuery(Guid Id) : IRequest<Result<GetMeResponse>>;

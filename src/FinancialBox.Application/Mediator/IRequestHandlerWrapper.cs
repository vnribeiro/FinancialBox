using FinancialBox.Application.Common;
using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Mediator;

internal interface IRequestHandlerWrapper<TResponse> where TResponse : IResult<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken);
}

using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Mediator;

internal interface IPipelineBehaviorWrapper<TResponse> where TResponse : IResult<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

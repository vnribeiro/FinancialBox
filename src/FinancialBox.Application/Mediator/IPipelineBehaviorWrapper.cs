using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Mediator;

internal interface IPipelineBehaviorWrapper<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

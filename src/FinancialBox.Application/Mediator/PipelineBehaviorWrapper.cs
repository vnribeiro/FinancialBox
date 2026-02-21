using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Mediator;

internal sealed class PipelineBehaviorWrapper<TRequest, TResponse>(IPipelineBehavior<TRequest, TResponse> inner)
    : IPipelineBehaviorWrapper<TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult<TResponse>
{
    private readonly IPipelineBehavior<TRequest, TResponse> _inner = inner;

    public Task<TResponse> Handle(IRequest<TResponse> request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => _inner.Handle((TRequest)request, next, cancellationToken);
}

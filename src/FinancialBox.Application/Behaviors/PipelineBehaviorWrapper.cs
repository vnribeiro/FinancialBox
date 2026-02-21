namespace FinancialBox.Application.Contracts.Messaging;

internal sealed class PipelineBehaviorWrapper<TRequest, TResponse>(IPipelineBehavior<TRequest, TResponse> inner) : IPipelineBehaviorWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPipelineBehavior<TRequest, TResponse> _inner = inner;

    public Task<TResponse> Handle(IRequest<TResponse> request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => _inner.Handle((TRequest)request, next, cancellationToken);
}

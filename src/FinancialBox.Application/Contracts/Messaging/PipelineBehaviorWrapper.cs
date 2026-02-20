namespace FinancialBox.Application.Contracts.Messaging;

internal interface IPipelineBehaviorWrapper<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, Func<Task<TResponse>> next, CancellationToken cancellationToken);
}

internal sealed class PipelineBehaviorWrapper<TRequest, TResponse> : IPipelineBehaviorWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPipelineBehavior<TRequest, TResponse> _inner;

    public PipelineBehaviorWrapper(IPipelineBehavior<TRequest, TResponse> inner)
        => _inner = inner;

    public Task<TResponse> Handle(IRequest<TResponse> request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
        => _inner.Handle((TRequest)request, next, cancellationToken);
}

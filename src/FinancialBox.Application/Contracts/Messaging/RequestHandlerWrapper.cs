namespace FinancialBox.Application.Contracts.Messaging;

internal sealed class RequestHandlerWrapper<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> inner) : IRequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner = inner;

    public Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken)
        => _inner.Handle((TRequest)request, cancellationToken);
}

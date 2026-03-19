using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Mediator;

internal sealed class RequestHandlerWrapper<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler)
    : IRequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _requestHandler = requestHandler;

    public Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken)
        => _requestHandler.Handle((TRequest)request, cancellationToken);
}

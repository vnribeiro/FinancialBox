using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Mediator;

internal sealed class RequestHandlerWrapper<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> inner)
    : IRequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner = inner;

    public Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken)
        => _inner.Handle((TRequest)request, cancellationToken);
}

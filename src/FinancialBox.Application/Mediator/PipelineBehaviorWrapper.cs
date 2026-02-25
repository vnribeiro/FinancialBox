using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Mediator;

internal sealed class PipelineBehaviorWrapper<TRequest, TResponse>(IPipelineBehavior<TRequest, TResponse> pipelineBehavior)
    : IPipelineBehaviorWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPipelineBehavior<TRequest, TResponse> _pipelineBehavior = pipelineBehavior;

    public Task<TResponse> Handle(IRequest<TResponse> request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => _pipelineBehavior.Handle((TRequest)request, next, cancellationToken);
}

namespace FinancialBox.Application.Contracts.Messaging;

internal interface IPipelineBehaviorWrapper<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

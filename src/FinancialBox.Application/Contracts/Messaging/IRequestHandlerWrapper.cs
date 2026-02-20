namespace FinancialBox.Application.Contracts.Messaging;

internal interface IRequestHandlerWrapper<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken);
}

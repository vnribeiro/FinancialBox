using FinancialBox.Application.Abstractions.Pipeline;

namespace FinancialBox.Application.Mediator;

internal interface IRequestHandlerWrapper<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken);
}

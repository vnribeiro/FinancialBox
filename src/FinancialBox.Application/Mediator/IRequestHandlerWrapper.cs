using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Application.Mediator;

internal interface IRequestHandlerWrapper<TResponse> where TResponse : IResult<TResponse>
{
    Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken);
}

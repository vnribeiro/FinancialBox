using FinancialBox.Application.Common;

namespace FinancialBox.Application.Contracts.Messaging;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

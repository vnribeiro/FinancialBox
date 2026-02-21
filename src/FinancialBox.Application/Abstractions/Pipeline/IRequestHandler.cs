using FinancialBox.Application.Common;

namespace FinancialBox.Application.Abstractions.Pipeline;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

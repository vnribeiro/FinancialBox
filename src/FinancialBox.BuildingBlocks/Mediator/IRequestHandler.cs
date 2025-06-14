using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<Result<TResponse>>
    where TResponse : notnull
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}
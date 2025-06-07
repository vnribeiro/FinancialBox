using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Behaviors;

public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<Result<TResponse>>
{
    Task<Result<TResponse>> Handle(TRequest request, Func<Task<Result<TResponse>>> next, CancellationToken cancellationToken);
}
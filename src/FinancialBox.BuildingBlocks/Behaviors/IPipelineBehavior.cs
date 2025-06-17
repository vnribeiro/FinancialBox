using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.BuildingBlocks.Behaviors;

public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken);
}
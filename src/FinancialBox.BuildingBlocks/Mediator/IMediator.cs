using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface IMediator
{
    Task<Result<TResponse>> SendAsync<TResponse>(IRequest<Result<TResponse>> request, CancellationToken cancellationToken);
}
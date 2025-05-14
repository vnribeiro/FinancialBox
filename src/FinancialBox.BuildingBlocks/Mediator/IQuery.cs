using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
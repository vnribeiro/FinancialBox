using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
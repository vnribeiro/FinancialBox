using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface ICommand : IRequestBase;
public interface ICommand<TResponse> : ICommand, IRequest<Result<TResponse>>;
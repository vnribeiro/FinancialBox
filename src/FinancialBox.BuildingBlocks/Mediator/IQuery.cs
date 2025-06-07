using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface IQuery : IRequestBase;
public interface IQuery<TResponse> : IQuery, IRequest<Result<TResponse>>;
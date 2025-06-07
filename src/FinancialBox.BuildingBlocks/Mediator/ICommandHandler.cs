using FinancialBox.BuildingBlocks.Result;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}
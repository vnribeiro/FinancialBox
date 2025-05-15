namespace FinancialBox.BuildingBlocks.Persistence;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}

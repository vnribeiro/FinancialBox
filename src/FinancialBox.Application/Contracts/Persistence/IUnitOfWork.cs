namespace FinancialBox.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}

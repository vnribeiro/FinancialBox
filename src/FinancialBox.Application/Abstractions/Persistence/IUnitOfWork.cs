namespace FinancialBox.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}

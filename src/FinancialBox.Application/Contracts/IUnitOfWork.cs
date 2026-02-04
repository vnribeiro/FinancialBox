namespace FinancialBox.Application.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}

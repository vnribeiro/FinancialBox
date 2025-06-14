using FinancialBox.BuildingBlocks.Primitives;

namespace FinancialBox.BuildingBlocks.Persistence;

public interface IRepository<T> where T : class, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Update(T entity);
    void Remove(T entity);
}

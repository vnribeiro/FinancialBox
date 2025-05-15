using FinancialBox.BuildingBlocks.DomainObjects;
using FinancialBox.BuildingBlocks.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

public abstract class Repository<T>(AppDbContext context) : 
    IRepository<T> where T : class, IAggregateRoot
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var obj = await _dbSet.AddAsync(entity, cancellationToken);
        return obj.Entity;
    }
    
    public T Update(T entity)
    {
        return _dbSet.Update(entity).Entity;
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}


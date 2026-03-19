using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal abstract class Repository<T>(AppDbContext context) :
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

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    { 
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    { 
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}


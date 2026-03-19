using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeEmailVerificationCodeRepository : IEmailVerificationCodeRepository
{
    private readonly List<Opt> _codes = [];

    public void Seed(Opt code) => _codes.Add(code);

    public Task<Opt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_codes.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Opt>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<Opt>>(_codes);

    public Task AddAsync(Opt entity, CancellationToken cancellationToken = default)
    {
        _codes.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(Opt entity) { }

    public void Remove(Opt entity) => _codes.Remove(entity);

    public virtual Task<Opt?> GetMostRecentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => Task.FromResult(
            _codes.Where(c => c.UserId == userId)
                  .OrderByDescending(c => c.CreatedAt)
                  .FirstOrDefault());

    public virtual Task<int> CountSentByUserIdAfterAsync(Guid userId, DateTime after, CancellationToken cancellationToken = default)
        => Task.FromResult(_codes.Count(c => c.UserId == userId && c.CreatedAt >= after));
}

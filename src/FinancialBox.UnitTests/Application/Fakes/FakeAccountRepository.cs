using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeAccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = [];

    public void Seed(Account account) => _accounts.Add(account);

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_accounts.FirstOrDefault(a => a.Id == id));

    public Task<IEnumerable<Account>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<Account>>(_accounts);

    public Task AddAsync(Account entity, CancellationToken cancellationToken = default)
    {
        _accounts.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(Account entity) { }

    public void Remove(Account entity) => _accounts.Remove(entity);

    public Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_accounts.FirstOrDefault(a => a.Email.Address == email));

    public Task<Account?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_accounts.FirstOrDefault(a => a.Email.Address == email));

    public Task<Account?> GetByEmailWithConfirmationTokensAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_accounts.FirstOrDefault(a => a.Email.Address == email));

    public Task<Account?> GetByIdWithRefreshTokensAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_accounts.FirstOrDefault(a => a.Id == id));

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_accounts.Any(a => a.Email.Address == email));
}

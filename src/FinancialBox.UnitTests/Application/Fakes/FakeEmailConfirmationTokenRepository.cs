using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeEmailConfirmationTokenRepository : IEmailConfirmationTokenRepository
{
    protected readonly List<EmailConfirmationToken> _tokens = [];

    public void Seed(EmailConfirmationToken token) => _tokens.Add(token);

    public Task AddAsync(EmailConfirmationToken token, CancellationToken cancellationToken = default)
    {
        _tokens.Add(token);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<EmailConfirmationToken>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<EmailConfirmationToken>>(_tokens);

    public Task<EmailConfirmationToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => Task.FromResult(_tokens.FirstOrDefault(t => t.TokenHash == tokenHash));

    public virtual Task<EmailConfirmationToken?> GetMostRecentByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        => Task.FromResult(
            _tokens.Where(t => t.AccountId == accountId)
                   .OrderByDescending(t => t.CreatedAt)
                   .FirstOrDefault());

    public virtual Task<int> CountSentByAccountIdAfterAsync(Guid accountId, DateTime after, CancellationToken cancellationToken = default)
        => Task.FromResult(_tokens.Count(t => t.AccountId == accountId && t.CreatedAt >= after));
}

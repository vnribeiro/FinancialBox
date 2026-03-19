using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class EmailConfirmationTokenRepository(AppDbContext context)
    : IEmailConfirmationTokenRepository
{
    public async Task AddAsync(EmailConfirmationToken token, CancellationToken cancellationToken = default)
    {
        await context.Set<EmailConfirmationToken>().AddAsync(token, cancellationToken);
    }

    public Task<EmailConfirmationToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => context.Set<EmailConfirmationToken>()
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

    public Task<EmailConfirmationToken?> GetMostRecentByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        => context.Set<EmailConfirmationToken>()
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<int> CountSentByAccountIdAfterAsync(Guid accountId, DateTime after, CancellationToken cancellationToken = default)
        => context.Set<EmailConfirmationToken>()
            .CountAsync(t => t.AccountId == accountId && t.CreatedAt >= after, cancellationToken);
}

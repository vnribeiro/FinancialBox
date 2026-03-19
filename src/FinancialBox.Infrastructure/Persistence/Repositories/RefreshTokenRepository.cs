using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class RefreshTokenRepository(AppDbContext context) :
    Repository<RefreshToken>(context), IRefreshTokenRepository
{
    private readonly AppDbContext _context = context;

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task RevokeAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.AccountId == accountId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        foreach (var token in tokens)
            token.Revoke();
    }
}

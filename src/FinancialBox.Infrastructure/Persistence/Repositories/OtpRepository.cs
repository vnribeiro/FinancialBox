using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class OtpRepository(AppDbContext context) :
    Repository<Otp>(context), IOtpRepository
{
    private readonly AppDbContext _context = context;

    public Task<Otp?> GetMostRecentByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return _context.Otps
            .Where(otp => otp.AccountId == accountId)
            .OrderByDescending(otp => otp.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> CountSentByAccountIdAfterAsync(Guid accountId, DateTime after, CancellationToken cancellationToken = default)
    {
        return _context.Otps
            .CountAsync(otp => otp.AccountId == accountId && otp.CreatedAt >= after, cancellationToken);
    }
}

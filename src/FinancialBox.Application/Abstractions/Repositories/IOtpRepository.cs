using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IOtpRepository : IRepository<Otp>
{
    Task<Otp?> GetMostRecentByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<int> CountSentByAccountIdAfterAsync(Guid accountId, DateTime after, CancellationToken cancellationToken = default);
}

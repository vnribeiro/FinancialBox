using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IEmailConfirmationTokenRepository
{
    Task AddAsync(EmailConfirmationToken token, CancellationToken cancellationToken = default);
    Task<EmailConfirmationToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<EmailConfirmationToken?> GetMostRecentByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<int> CountSentByAccountIdAfterAsync(Guid accountId, DateTime after, CancellationToken cancellationToken = default);
}

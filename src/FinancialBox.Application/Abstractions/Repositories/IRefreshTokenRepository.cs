using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
}

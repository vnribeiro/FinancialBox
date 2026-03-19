using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByEmailWithConfirmationTokensAsync(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByConfirmationTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<Account?> GetByIdWithRefreshTokensAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}

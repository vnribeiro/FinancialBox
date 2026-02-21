using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Contracts.Repositories;

public interface IEmailVerificationRepository : IRepository<EmailVerification>
{
    Task<EmailVerification?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Contracts.Repositories;

public interface IEmailVerificationCodeRepository : IRepository<EmailVerification>
{
    Task<EmailVerification?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

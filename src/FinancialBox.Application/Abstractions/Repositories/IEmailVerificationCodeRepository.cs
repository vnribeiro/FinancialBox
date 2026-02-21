using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IEmailVerificationCodeRepository : IRepository<EmailVerificationCode>
{
    Task<EmailVerificationCode?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

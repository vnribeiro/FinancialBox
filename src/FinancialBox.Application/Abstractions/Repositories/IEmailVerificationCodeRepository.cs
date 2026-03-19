using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IEmailVerificationCodeRepository : IRepository<EmailVerificationCode>
{
    Task<EmailVerificationCode?> GetMostRecentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> CountSentByUserIdAfterAsync(Guid userId, DateTime after, CancellationToken cancellationToken = default);
}

using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IEmailVerificationCodeRepository : IRepository<Opt>
{
    Task<Opt?> GetMostRecentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> CountSentByUserIdAfterAsync(Guid userId, DateTime after, CancellationToken cancellationToken = default);
}

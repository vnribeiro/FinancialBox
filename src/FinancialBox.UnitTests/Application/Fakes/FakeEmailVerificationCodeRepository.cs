using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeEmailVerificationCodeRepository : IEmailVerificationCodeRepository
{
    private readonly List<EmailVerificationCode> _codes = [];

    public void Seed(EmailVerificationCode code) => _codes.Add(code);

    public Task<EmailVerificationCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_codes.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<EmailVerificationCode>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<EmailVerificationCode>>(_codes);

    public Task AddAsync(EmailVerificationCode entity, CancellationToken cancellationToken = default)
    {
        _codes.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(EmailVerificationCode entity) { }

    public void Remove(EmailVerificationCode entity) => _codes.Remove(entity);

    public virtual Task<EmailVerificationCode?> GetMostRecentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => Task.FromResult(
            _codes.Where(c => c.UserId == userId)
                  .OrderByDescending(c => c.CreatedAt)
                  .FirstOrDefault());

    public virtual Task<int> CountSentByUserIdAfterAsync(Guid userId, DateTime after, CancellationToken cancellationToken = default)
        => Task.FromResult(_codes.Count(c => c.UserId == userId && c.CreatedAt >= after));
}

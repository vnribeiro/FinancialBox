using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Users;

public class EmailVerification : AggregateRoot
{
    private const int MaxAttempts = 5;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string CodeHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }
    public int Attempts { get; private set; }

    protected EmailVerification() {}

    public EmailVerification(Guid userId, string codeHash, DateTime expiresAt)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(codeHash))
        {
            throw new ArgumentException("CodeHash cannot be empty.", nameof(codeHash));
        }

        UserId = userId;
        CodeHash = codeHash;
        ExpiresAt = expiresAt;
        Attempts = 0;
    }

    public bool CanValidate(DateTime utcNow)
    {
        return UsedAt is null && ExpiresAt >= utcNow && Attempts < MaxAttempts;
    }

    public void RegisterFailedAttempt(DateTime utcNow)
    {
        Attempts++;
        UpdatedAt = utcNow;
    }

    public void MarkAsUsed(DateTime utcNow)
    {
        UsedAt = utcNow;
        UpdatedAt = utcNow;
    }
}

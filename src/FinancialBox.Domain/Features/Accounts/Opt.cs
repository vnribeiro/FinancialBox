using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Accounts;

public class Otp : BaseEntity
{
    public Guid AccountId { get; private set; }
    public string CodeHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }
    public int Attempts { get; private set; }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAt;
    public bool IsUsed => UsedAt is not null;

    protected Otp() { }

    private Otp(Guid accountId, string codeHash, DateTime expiresAt)
    {
        AccountId = accountId;
        CodeHash = codeHash;
        ExpiresAt = expiresAt;
    }

    public static Otp Create(Guid accountId, string codeHash, DateTime expiresAt)
        => new(accountId, codeHash, expiresAt);

    public bool CanValidate(DateTime utcNow, int maxAttempts)
        => !IsUsed && !IsExpired(utcNow) && Attempts < maxAttempts;

    public void RegisterFailedAttempt() => Attempts++;

    public void MarkAsUsed(DateTime utcNow) => UsedAt = utcNow;
}
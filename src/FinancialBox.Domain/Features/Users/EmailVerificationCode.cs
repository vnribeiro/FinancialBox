using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.Users.Events;

namespace FinancialBox.Domain.Features.Users;

public class EmailVerificationCode : AggregateRoot
{
    public Guid UserId { get; private set; }
    public string CodeHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }
    public int Attempts { get; private set; }

    protected EmailVerificationCode() {}

    private EmailVerificationCode(Guid userId, string codeHash, DateTime expiresAt)
    {
        UserId = userId;
        CodeHash = codeHash;
        ExpiresAt = expiresAt;
    }

    public bool CanValidate(DateTime utcNow, int maxAttempts)
    {
        return UsedAt is null && ExpiresAt >= utcNow && Attempts < maxAttempts;
    }

    public void RegisterFailedAttempt()
    {
        Attempts++;
    }

    public void MarkAsUsed(DateTime utcNow)
    {
        UsedAt = utcNow;
    }

    public static EmailVerificationCode Create(Guid userId, string email, string plainCode, string codeHash, DateTime expiresAt)
    {
        var code = new EmailVerificationCode(userId, codeHash, expiresAt);
        code.AddDomainEvent(new EmailVerificationCodeCreatedEvent(userId, email, plainCode));
        return code;
    }
}

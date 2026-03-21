using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Accounts;

public class EmailConfirmationToken : BaseEntity
{
    public Guid AccountId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAt;
    public bool IsUsed => UsedAt is not null;

    protected EmailConfirmationToken() {}

    private EmailConfirmationToken(Guid accountId, DateTime expiresAt)
    {
        AccountId = accountId;
        Token = new Guid().ToString();
        ExpiresAt = expiresAt;
    }

    public static EmailConfirmationToken Create(Guid accountId, DateTime expiresAt)
        => new(accountId, expiresAt);

    public bool CanValidate(DateTime utcNow)
        => !IsUsed && !IsExpired(utcNow);

    public void MarkAsUsed(DateTime utcNow) => UsedAt = utcNow;
}

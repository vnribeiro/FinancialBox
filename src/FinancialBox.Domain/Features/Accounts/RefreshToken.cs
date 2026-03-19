using FinancialBox.Domain.Common;

namespace FinancialBox.Domain.Features.Accounts;

public class RefreshToken : AggregateRoot
{
    public Guid AccountId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt is not null;
    public bool IsActive => !IsExpired && !IsRevoked;

    protected RefreshToken() {}

    private RefreshToken(Guid accountId, string token, DateTime expiresAt)
    {
        AccountId = accountId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(Guid accountId, string token, DateTime expiresAt)
        => new(accountId, token, expiresAt);

    public void Revoke() => RevokedAt = DateTime.UtcNow;
}
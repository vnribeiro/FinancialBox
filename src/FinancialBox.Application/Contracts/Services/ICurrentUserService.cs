namespace FinancialBox.Application.Contracts.Services;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? Email { get; }
    string? Name { get; }
    IReadOnlyList<string> Roles { get; }
    DateTimeOffset? IssuedAt { get; }
    DateTimeOffset? ExpiresAt { get; }
    TimeSpan? TimeToExpire { get; }
}

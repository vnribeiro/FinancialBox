using FinancialBox.Domain.Features.User;

namespace FinancialBox.Application.Contracts.Services;

public interface IJwtService
{
    TokenResponse GenerateToken(User user);
}

public sealed record TokenResponse(string AccessToken, DateTime ExpiresAtUtc);

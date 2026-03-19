using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Abstractions.Services;

public interface IJwtService
{
    TokenResponse GenerateToken(User user);
}

public sealed record TokenResponse(string AccessToken, DateTime ExpiresAtUtc);

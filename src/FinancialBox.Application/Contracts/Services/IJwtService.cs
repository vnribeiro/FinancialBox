using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Contracts.Services;

public interface IJwtService
{
    TokenResponse GenerateToken(User user);
}

public sealed record TokenResponse(string AccessToken, int ExpiresInSeconds);

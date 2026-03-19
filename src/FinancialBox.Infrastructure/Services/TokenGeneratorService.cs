using System.Security.Cryptography;
using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.Infrastructure.Services;

internal sealed class TokenGeneratorService : ITokenGeneratorService
{
    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}

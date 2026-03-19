using System.Security.Cryptography;
using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.Infrastructure.Services;

internal sealed class TokenGeneratorService : ITokenGeneratorService
{
    public string GenerateOtp(int digits = 6)
    {
        var max = (int)Math.Pow(10, digits);
        return RandomNumberGenerator.GetInt32(0, max).ToString($"D{digits}");
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}

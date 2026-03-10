using System.Security.Cryptography;
using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.Application.Features.Auth;

public static class OtpGenerator
{
    public static (string PlainCode, string CodeHash) Generate(ISecureHashService secureHashService, int digits = 6)
    {
        ArgumentNullException.ThrowIfNull(secureHashService);

        if (digits is <= 0 or > 9)
            throw new ArgumentOutOfRangeException(nameof(digits), "OTP digits must be between 1 and 9.");

        var max = (int)Math.Pow(10, digits);
        var plainCode = RandomNumberGenerator.GetInt32(0, max).ToString($"D{digits}");
        var codeHash = secureHashService.Hash(plainCode);

        return (plainCode, codeHash);
    }
}

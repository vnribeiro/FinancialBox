using System.Security.Cryptography;

namespace FinancialBox.Application.Features.Auth;

public static class OtpGenerator
{
    public static string Generate(int digits = 6)
    {
        var max = (int)Math.Pow(10, digits);
        return RandomNumberGenerator.GetInt32(0, max).ToString($"D{digits}");
    }
}

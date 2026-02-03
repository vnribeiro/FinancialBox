using System.Security.Cryptography;
using FinancialBox.Application.Contracts.Services;
using Microsoft.Extensions.Options;

namespace FinancialBox.Infrastructure.Services;

internal sealed class PasswordHasherService(IOptions<PasswordHashingOptions> options) : IPasswordHasherService
{
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    private readonly PasswordHashingOptions _options = options.Value;

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
        var subkey = Pbkdf2(password, salt, _options.Iterations, _options.SubkeySize);

        return $"PBKDF2${Algorithm.Name}${_options.Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(subkey)}";
    }

    public bool Verify(string hash, string password)
    {
        if (!TryParseHash(hash, _options.SaltSize, _options.SubkeySize, out var iterations, out var salt, out var expectedSubkey))
            return false;

        var actualSubkey = Pbkdf2(password, salt, iterations, expectedSubkey.Length);
        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }

    private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int subkeyLength)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, Algorithm);
        return pbkdf2.GetBytes(subkeyLength);
    }

    private static bool TryParseHash(string hash, int expectedSaltSize, int expectedSubkeySize, out int iterations, out byte[] salt, out byte[] subkey)
    {
        iterations = 0;
        salt = Array.Empty<byte>();
        subkey = Array.Empty<byte>();

        var parts = hash.Split('$');
        if (parts.Length != 5)
            return false;

        if (!parts[0].Equals("PBKDF2", StringComparison.Ordinal) ||
            !parts[1].Equals(Algorithm.Name, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!int.TryParse(parts[2], out iterations) || iterations <= 0)
            return false;

        try
        {
            salt = Convert.FromBase64String(parts[3]);
            subkey = Convert.FromBase64String(parts[4]);
        }
        catch (FormatException)
        {
            return false;
        }

        return salt.Length == expectedSaltSize && subkey.Length == expectedSubkeySize;
    }
}

internal sealed record PasswordHashingOptions(int Iterations, int SaltSize, int SubkeySize);
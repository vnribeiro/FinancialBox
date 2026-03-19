using System.Security.Cryptography;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Infrastructure.Services.Options;
using Microsoft.Extensions.Options;

namespace FinancialBox.Infrastructure.Services;

internal sealed class HasherService(IOptions<HasherOptions> options) : IHasherService
{
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    private readonly HasherOptions _options = options.Value;

    public string Hash(string raw)
    {
        var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);

        var subkey = Rfc2898DeriveBytes.Pbkdf2(
            raw, 
            salt, 
            _options.Iterations, 
            Algorithm, 
            _options.SubkeySize);
        
        return $"PBKDF2${Algorithm.Name}${_options.Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(subkey)}";
    }

    public bool Verify(string hash, string raw)
    {
        if (!TryParseHash(hash, _options.SaltSize, _options.SubkeySize, out var iterations, out var salt, out var expectedSubkey))
            return false;

        var actualSubkey = Rfc2898DeriveBytes.Pbkdf2(
            raw,
            salt,
            iterations,
            Algorithm,
            expectedSubkey.Length);

        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }

    private static bool TryParseHash(string hash, int expectedSaltSize, int expectedSubkeySize, out int iterations, out byte[] salt, out byte[] subkey)
    {
        iterations = 0;
        salt = [];
        subkey = [];

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
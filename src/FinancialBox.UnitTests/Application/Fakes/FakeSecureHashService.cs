using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.UnitTests.Application.Fakes;

/// <summary>
/// Identity hash: Hash(x) == x, Verify(hash, value) == (hash == value).
/// Keeps tests readable without real crypto.
/// </summary>
public class FakeSecureHashService : ISecureHashService
{
    public string Hash(string value) => value;

    public bool Verify(string hash, string value) => hash == value;
}

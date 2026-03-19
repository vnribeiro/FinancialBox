using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.UnitTests.Application.Fakes;

/// <summary>
/// Identity hash: Hash(x) == x, Verify(hash, value) == (hash == value).
/// </summary>
public class FakeHasherService : IHasherService
{
    public string Hash(string value) => value;
    public bool Verify(string hash, string value) => hash == value;
}

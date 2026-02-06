namespace FinancialBox.Domain.Features.User.ValueObjects;

public sealed class Password : IEquatable<Password>
{
    public string Hash { get; }

    private Password(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(hash));

        Hash = hash;
    }

    // Factory method to create a Password from a hash
    public static Password FromHash(string hash)
        => new(hash);

    public bool Equals(Password? other)
        => other is not null && Hash == other.Hash;

    public override bool Equals(object? obj)
        => Equals(obj as Password);

    public override int GetHashCode()
        => Hash.GetHashCode(StringComparison.Ordinal);

    public static bool operator ==(Password? left, Password? right)
        => Equals(left, right);

    public static bool operator !=(Password? left, Password? right)
        => !Equals(left, right);

    public override string ToString()
        => "[PROTECTED]";
}

using System.Text.RegularExpressions;

namespace FinancialBox.Domain.Features.Users.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Address { get; }

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Email address cannot be empty.", nameof(address));

        address = address.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(address))
            throw new ArgumentException("Invalid email address format.", nameof(address));

        Address = address;
    }

    public bool Equals(Email? other)
        => other is not null && Address == other.Address;

    public override bool Equals(object? obj)
        => Equals(obj as Email);

    public override int GetHashCode()
        => Address.GetHashCode(StringComparison.Ordinal);

    public static bool operator ==(Email? left, Email? right)
        => Equals(left, right);

    public static bool operator !=(Email? left, Email? right)
        => !Equals(left, right);

    public override string ToString()
        => Address;
}
using FinancialBox.Domain.Primitives;
using System.Text.RegularExpressions;
using FinancialBox.Domain.Features.Users.Errors;

namespace FinancialBox.Domain.Features.Users.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Address { get; }

    public static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string address) => Address = address;

    public static Result<Email> Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return EmailErrors.Empty;

        return !EmailRegex.IsMatch(address) ?
            EmailErrors.InvalidFormat : 
            Result<Email>.Success(new Email(address));
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
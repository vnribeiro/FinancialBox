using System.Text.RegularExpressions;

namespace FinancialBox.Domain.Features.Users.ValueObjects;

public sealed class Cpf : IEquatable<Cpf>
{
    public string Number { get; }

    private static readonly Regex OnlyDigits = new(@"^\d{11}$", RegexOptions.Compiled);

    public Cpf(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("CPF cannot be empty.", nameof(number));

        number = Normalize(number);

        if (!IsValid(number))
            throw new ArgumentException("Invalid CPF.", nameof(number));

        Number = number;
    }

    private static string Normalize(string value)
        => Regex.Replace(value, @"\D", "");

    private static bool IsValid(string cpf)
    {
        if (!OnlyDigits.IsMatch(cpf))
            return false;

        // Reject repeated digits (11111111111, etc.)
        if (new string(cpf[0], 11) == cpf)
            return false;

        var digits = cpf.Select(c => c - '0').ToArray();

        // First check digit
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += digits[i] * (10 - i);

        var remainder = sum % 11;
        var firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        if (digits[9] != firstCheckDigit)
            return false;

        // Second check digit
        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += digits[i] * (11 - i);

        remainder = sum % 11;
        var secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return digits[10] == secondCheckDigit;
    }

    public bool Equals(Cpf? other)
        => other is not null && Number == other.Number;

    public override bool Equals(object? obj)
        => Equals(obj as Cpf);

    public override int GetHashCode()
        => Number.GetHashCode(StringComparison.Ordinal);

    public static bool operator ==(Cpf? left, Cpf? right)
        => Equals(left, right);

    public static bool operator !=(Cpf? left, Cpf? right)
        => !Equals(left, right);

    public override string ToString()
        => Number;
}

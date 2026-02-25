using FinancialBox.Domain.Primitives;

namespace FinancialBox.Domain.Features.Users.Errors;

public static class EmailErrors
{
    public static Error Empty =>
        Error.Validation("Email.Empty", "Email address cannot be empty.");

    public static Error InvalidFormat =>
        Error.Validation("Email.InvalidFormat", "Email address has an invalid format.");
}

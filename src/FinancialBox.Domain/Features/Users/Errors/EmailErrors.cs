using FinancialBox.Domain.Primitives;

namespace FinancialBox.Domain.Features.Users.Errors;

public static class EmailErrors
{
    public static Error Empty =>
        Error.Validation("EMAIL_EMPTY", "Email address cannot be empty.");

    public static Error InvalidFormat =>
        Error.Validation("EMAIL_INVALID_FORMAT", "Email address has an invalid format.");
}

using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials =>
        Error.Unauthenticated("Auth.InvalidCredentials", "Invalid email or password.");

    public static Error EmailNotConfirmed =>
        Error.Unauthenticated("Auth.EmailNotConfirmed", "Email address is not confirmed.");

    public static Error EmailAlreadyExists =>
        Error.Conflict("Auth.EmailAlreadyExists", "Email address is already in use.");

    public static Error InvalidOrExpiredCode =>
        Error.Unauthenticated("Auth.InvalidOrExpiredCode", "The verification code is invalid or has expired.");

    public static Error ResendLimitReached =>
        Error.TooManyRequests("Auth.ResendLimitReached", "Too many confirmation emails sent. Please wait before trying again.");
}

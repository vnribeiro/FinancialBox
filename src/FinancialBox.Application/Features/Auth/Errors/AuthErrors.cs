using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials =>
        Error.Unauthenticated("AUTH_INVALID_CREDENTIALS", "Invalid email or password.");

    public static Error EmailNotConfirmed =>
        Error.Unauthenticated("AUTH_EMAIL_NOT_CONFIRMED", "Email address is not confirmed.");

    public static Error InvalidOrExpiredCode =>
        Error.Unauthenticated("AUTH_INVALID_OR_EXPIRED_CODE", "The verification code is invalid or has expired.");

    public static Error ResendLimitReached =>
        Error.TooManyRequests("AUTH_RESEND_LIMIT_REACHED", "Too many confirmation emails sent. Please wait before trying again.");
}

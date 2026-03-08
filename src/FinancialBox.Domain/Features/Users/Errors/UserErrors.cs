using FinancialBox.Domain.Primitives;

namespace FinancialBox.Domain.Features.Users.Errors;

public static class UserErrors
{
    public static Error NotIdentified =>
        Error.Unauthenticated("USER_NOT_IDENTIFIED", "User identity could not be determined.");

    public static Error NotFound =>
        Error.NotFound("USER_NOT_FOUND", "User was not found.");

    public static Error EmailAlreadyInUse =>
        Error.Conflict("USER_EMAIL_ALREADY_IN_USE", "Email address is already in use.");
}


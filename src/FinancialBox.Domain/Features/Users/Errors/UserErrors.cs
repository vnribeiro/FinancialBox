using FinancialBox.Domain.Primitives;

namespace FinancialBox.Domain.Features.Users.Errors;

public static class UserErrors
{
    public static Error NotIdentified =>
        Error.Unauthenticated("User.NotIdentified", "User identity could not be determined.");

    public static Error NotFound =>
        Error.NotFound("User.NotFound", "User was not found.");

    public static Error EmailAlreadyInUse =>
        Error.Conflict("User.EmailAlreadyInUse", "Email address is already in use.");
}


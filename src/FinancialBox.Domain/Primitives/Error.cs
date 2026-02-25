namespace FinancialBox.Domain.Primitives;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = string.IsNullOrEmpty(message) && type != ErrorType.None ? DefaultMessagesFor(type) : message;
        Type = type;
    }

    private static string DefaultMessagesFor(ErrorType type) => type switch
    {
        ErrorType.Validation         => "One or more validation rules failed.",
        ErrorType.BusinessRule       => "The request could not be processed.",
        ErrorType.Unauthenticated    => "Authentication is required.",
        ErrorType.Forbidden          => "You do not have permission to access this resource.",
        ErrorType.NotFound           => "The requested resource was not found.",
        ErrorType.Conflict           => "A conflict occurred with the current state of the resource.",
        ErrorType.TooManyRequests    => "Too many requests. Please try again later.",
        ErrorType.InternalError      => "An unexpected error occurred on the server.",
        ErrorType.NotImplemented     => "This feature is not yet available.",
        ErrorType.ServiceUnavailable => "The service is temporarily unavailable.",
        _                            => "An unknown error occurred."
    };

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error BusinessRule(string code, string message) =>
        new(code, message, ErrorType.BusinessRule);

    public static Error Unauthenticated(string code, string message) =>
        new(code, message, ErrorType.Unauthenticated);

    public static Error Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static Error TooManyRequests(string code, string message) =>
        new(code, message, ErrorType.TooManyRequests);

    public static Error InternalError(string code, string message) =>
        new(code, message, ErrorType.InternalError);

    public static Error NotImplemented(string code, string message) =>
        new(code, message, ErrorType.NotImplemented);

    public static Error ServiceUnavailable(string code, string message) =>
        new(code, message, ErrorType.ServiceUnavailable);
}

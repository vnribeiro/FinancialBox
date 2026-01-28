namespace FinancialBox.Application.Common;

public enum ErrorType
{
    InvalidRequest = 400, // Bad Request
    AuthenticationRequired = 401, // Unauthorized
    AccessDenied = 403, // Forbidden
    ResourceNotFound = 404, // Not Found
    ResourceConflict = 409, // Conflict
    ValidationFailure = 422, // Unprocessable Entity
    RateLimitExceeded = 429, // Too Many Requests
    UnexpectedServerError = 500, // Internal Server Error
    FeatureNotAvailable = 501, // Not Implemented (feature not yet ready)
    ServiceTemporarilyUnavailable = 503  // Service Unavailable
}

public class Error
{
    public ErrorType Type { get; }
    public int StatusCode { get; }
    public IReadOnlyList<string> Messages { get; }

    private Error(ErrorType type, params string[] messages)
    {
        Type = type;
        StatusCode = (int)type;
        Messages = messages.Length > 0 ? messages.ToList() : DefaultMessagesFor(type);
    }

    private static List<string> DefaultMessagesFor(ErrorType type) => type switch
    {
        ErrorType.InvalidRequest => ["The request is invalid."],
        ErrorType.AuthenticationRequired => ["Authentication is required."],
        ErrorType.AccessDenied => ["You do not have permission to access this resource."],
        ErrorType.ResourceNotFound => ["The requested resource was not found."],
        ErrorType.ResourceConflict => ["A conflict occurred with the current state of the resource."],
        ErrorType.ValidationFailure => ["One or more validation rules failed."],
        ErrorType.RateLimitExceeded => ["Too many requests. Please try again later."],
        ErrorType.FeatureNotAvailable => ["This feature is not yet available."],
        ErrorType.ServiceTemporarilyUnavailable => ["The service is temporarily unavailable."],
        ErrorType.UnexpectedServerError => ["An unexpected error occurred on the server."],
        _ => ["An unknown error occurred."]
    };

    public static Error InvalidRequest(params string[] messages) => 
        new(ErrorType.InvalidRequest, messages);

    public static Error AuthenticationRequired(params string[] messages) => 
        new(ErrorType.AuthenticationRequired, messages);

    public static Error AccessDenied(params string[] messages) => 
        new(ErrorType.AccessDenied, messages);

    public static Error ResourceNotFound(params string[] messages) => 
        new(ErrorType.ResourceNotFound, messages);

    public static Error ResourceConflict(params string[] messages) => 
        new(ErrorType.ResourceConflict, messages);

    public static Error ValidationFailure(params string[] messages) => 
        new(ErrorType.ValidationFailure, messages);
    public static Error RateLimitExceeded(params string[] messages) => 
        new(ErrorType.RateLimitExceeded, messages);

    public static Error FeatureNotAvailable(params string[] messages) => 
        new(ErrorType.FeatureNotAvailable, messages);

    public static Error ServiceTemporarilyUnavailable(params string[] messages) => 
        new(ErrorType.ServiceTemporarilyUnavailable, messages);

    public static Error UnexpectedServerError(params string[] messages) => 
        new(ErrorType.UnexpectedServerError, messages);

    public override string ToString() => string.Join(" | ", Messages);
}

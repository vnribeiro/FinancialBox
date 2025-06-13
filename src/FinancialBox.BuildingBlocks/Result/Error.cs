namespace FinancialBox.BuildingBlocks.Result;

public enum ErrorType
{
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    UnprocessableEntity = 422,
    TooManyRequests = 429,
    InternalServerError = 500,
    NotImplemented = 501,
    ServiceUnavailable = 503
}


public class Error
{
    public ErrorType Type { get; init; }
    public int StatusCode { get; init; }
    public IReadOnlyList<string> Messages { get; }

    private Error(ErrorType type, params string[] messages)
    {
        Type = type;
        StatusCode = (int)type;
        Messages = messages.Length != 0 ? messages.ToList() : DefaultMessagesFor(type);
    }

    private static List<string> DefaultMessagesFor(ErrorType type) => type switch
    {
        ErrorType.BadRequest => ["The request is invalid."],
        ErrorType.Unauthorized => ["Authentication is required."],
        ErrorType.Forbidden => ["Access to this resource is forbidden."],
        ErrorType.NotFound => ["The requested resource was not found."],
        ErrorType.Conflict => ["A conflict occurred in the request."],
        ErrorType.UnprocessableEntity => ["One or more fields are invalid."],
        ErrorType.TooManyRequests => ["Too many requests. Try again later."],
        ErrorType.NotImplemented => ["This feature is not implemented."],
        _ => ["An unexpected error occurred."]
    };

    public static Error BadRequest(params string[] messages) =>
        new(ErrorType.BadRequest, messages);

    public static Error NotFound(params string[] messages) =>
        new(ErrorType.NotFound, messages);

    public static Error Conflict(params string[] messages) =>
        new(ErrorType.Conflict, messages);

    public static Error Unauthorized(params string[] messages) =>
        new(ErrorType.Unauthorized, messages);

    public static Error Forbidden(params string[] messages) =>
        new(ErrorType.Forbidden, messages);

    public static Error InternalServerError(params string[] messages) =>
        new(ErrorType.InternalServerError, messages);

    public static Error UnprocessableEntity(params string[] messages) =>
        new(ErrorType.UnprocessableEntity, messages);

    public static Error TooManyRequests(params string[] messages) =>
        new(ErrorType.TooManyRequests, messages);

    public static Error NotImplemented(params string[] messages) =>
        new(ErrorType.NotImplemented, messages);

    public static Error ServiceUnavailable(params string[] messages) =>
        new(ErrorType.ServiceUnavailable, messages);

    public override string ToString()
    {
        return string.Join(" | ", Messages);
    }
}
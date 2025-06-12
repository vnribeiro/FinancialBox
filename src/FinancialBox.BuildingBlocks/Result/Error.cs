namespace FinancialBox.BuildingBlocks.Result;

public enum ErrorType
{
    BadRequest,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}

public class Error
{
    public ErrorType Type { get; init; }
    public int StatusCode { get; init; }
    public IReadOnlyList<string> Messages { get; }

    public Error(int statusCode, ErrorType type, params string[] messages)
    {
        StatusCode = statusCode;
        Type = type;
        Messages = messages?.ToList() ?? [];
    }

    public static Error BadRequest(params string[] messages) =>
        new(400, ErrorType.BadRequest, messages);

    public static Error NotFound(params string[] messages) =>
        new(404, ErrorType.NotFound, messages);

    public static Error Conflict(params string[] messages) =>
        new(409, ErrorType.Conflict, messages);

    public static Error Unexpected(params string[] messages) =>
        new(500, ErrorType.Unexpected, messages.Length > 0 ? messages : ["An unexpected error occurred."]);

    public override string ToString()
    {
        return string.Join(" | ", Messages);
    }
}
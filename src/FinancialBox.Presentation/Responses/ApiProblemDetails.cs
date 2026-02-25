using FinancialBox.Domain.Primitives;

namespace FinancialBox.Presentation.Responses;

public class ApiProblemDetails
{
    public string Type { get; }
    public string Title { get; }
    public int Status { get; }
    public IReadOnlyList<ApiError> Errors { get; }

    private ApiProblemDetails(string type, string title, int status, IReadOnlyList<ApiError> errors)
    {
        Type = type;
        Title = title;
        Status = status;
        Errors = errors;
    }

    public static ApiProblemDetails FromErrors(IReadOnlyList<Error> errors, int statusCode) =>
        new(type: "about:blank",
            title: GetTitle(statusCode),
            status: statusCode,
            errors: [.. errors.Select(e => new ApiError(e.Code, e.Message))]);

    private static string GetTitle(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        422 => "Unprocessable Entity",
        429 => "Too Many Requests",
        500 => "Internal Server Error",
        501 => "Not Implemented",
        503 => "Service Unavailable",
        _   => "An error occurred"
    };
}
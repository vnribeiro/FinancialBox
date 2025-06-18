namespace FinancialBox.Presentation.Responses;

public class ApiResponse
{
    public bool Success { get; }
    public IReadOnlyList<string> Errors { get; }

    private ApiResponse(bool success, IReadOnlyList<string> errors)
    {
        Success = success;
        Errors = errors;
    }

    public static ApiResponse FromErrors(IReadOnlyList<string> errors) =>
        new(false, errors);
}

public class ApiResponse<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public IReadOnlyList<string> Errors { get; }

    private ApiResponse(bool success, T? data, IReadOnlyList<string> errors)
    {
        Success = success;
        Data = data;
        Errors = errors;
    }

    public static ApiResponse<T> FromSuccess(T data) => 
        new(true, data, []);

    public static ApiResponse<T> FromErrors(IReadOnlyList<string> errors) => 
        new(false, default, errors);
}
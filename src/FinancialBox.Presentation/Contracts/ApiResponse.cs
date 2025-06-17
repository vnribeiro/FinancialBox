namespace FinancialBox.Presentation.Contracts;

public class ApiResponse
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = [];

    public static ApiResponse FromErrors(IEnumerable<string> errors) =>
        new() { Success = false, Errors = errors.ToList() };
}

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];

    public static ApiResponse<T> FromSuccess(T data) => 
        new() { Success = true, Data = data, Errors = [] };

    public static ApiResponse<T> FromErrors(IEnumerable<string> errors) => 
        new() { Success = false, Data = default, Errors = errors?.ToList() ?? [] };
}
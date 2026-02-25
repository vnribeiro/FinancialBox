using System.Text.Json.Serialization;

namespace FinancialBox.Presentation.Responses;

public class ApiResponse
{
    protected ApiResponse() {}

    public static ApiResponse Success() => new();
}

public class ApiResponse<T> : ApiResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; }

    private ApiResponse(T data)
    {
        Data = data;
    }

    public static ApiResponse<T> Success(T data) =>
        new(data);
}

using System.Text.Json.Serialization;

namespace exam_system.Features.Shared;

public class ApiResponse
{
    [JsonPropertyOrder(0)]
    public bool Success { get; protected set; }
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; protected set; } = string.Empty;
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ApiMeta? Meta { get; protected set; }

    public static ApiResponse Ok(string? message, string traceId)
        => new()
        {
            Success = true,
            Message = message,
            Meta = new()
            {
                TraceId = traceId,
            }
        };
}

public sealed class ApiResponse<TData> : ApiResponse
{
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TData? Data { get; private set; } 

    public static ApiResponse<TData> Ok(TData data, string? message, string traceId, PaginationMeta? paginationMeta = null)
        => new()
        {
            Success = true,
            Data = data,
            Message = message,
            Meta = new()
            {
                TraceId = traceId,
                Pagination = paginationMeta
            }
        };
}

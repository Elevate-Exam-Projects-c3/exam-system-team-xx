using System.Text.Json.Serialization;

namespace exam_system.Features.Shared;

public sealed class ApiMeta
{
    public string TraceId { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PaginationMeta? Pagination { get;  set; }
}
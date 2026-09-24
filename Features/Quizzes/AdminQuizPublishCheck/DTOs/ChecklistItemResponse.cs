using System.Text.Json.Serialization;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;

public sealed class ChecklistItemResponse
{
    public string Name { get; set; } = default!;
    public bool Success { get; set; } = true;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; } 
}

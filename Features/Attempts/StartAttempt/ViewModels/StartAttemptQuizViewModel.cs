using exam_system.Common.Enums;
using exam_system.Features.Attempts.StartAttempt.DTOs;
using System.Text.Json.Serialization;

namespace exam_system.Features.Attempts.StartAttempt.ViewModels;

public sealed class StartAttemptQuizViewModel
{
    public string QuizTitle { get; set; } = default!;
    public string? QuizInstructions { get; set; }
    public int QuizDurationInMinutes { get; set; }
    public int QuizPassScore { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? QuizMaxAttempts { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? StudentQuizRemainingAttempts { get; set; }
    public ICollection<StartAttemptQuestionViewModel> Questions { get; set; } = [];
}

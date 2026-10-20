using System.Collections;

namespace exam_system.Features.Attempts.StartAttempt.DTOs;

public sealed class StartAttemptQuestionResponse
{
    public string QuestionText { get; set; } = default!;
    public ICollection<string> QuestionOptions { get; set; } = [];
}

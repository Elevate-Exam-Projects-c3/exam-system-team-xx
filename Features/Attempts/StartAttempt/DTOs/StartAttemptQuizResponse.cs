using exam_system.Common.Enums;

namespace exam_system.Features.Attempts.StartAttempt.DTOs;

public sealed class StartAttemptQuizResponse
{
    public Guid DiplomaId { get; set; }
    public string QuizTitle { get; set; } = default!;
    public string? QuizInstructions { get; set; }
    public int QuizDurationInMinutes { get; set; }
    public QuizStatus QuizStatus { get; set; }
    public int QuizPassScore { get; set; }
    public int? QuizMaxAttempts { get; set; }
    public int StudentQuizAttempts { get; set; } = 0;
    public ICollection<StartAttemptQuestionResponse> Questions { get; set; } = [];
}

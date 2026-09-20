namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Requests;

public sealed class AdminUpdateQuizRequest
{
    public string? Title { get; set; }
    public string? Instructions { get; set; } 
    public int? DurationMinutes { get; set; }
    public int? PassScore { get; set; }
    public int? MaxAttempts { get; set; }
}

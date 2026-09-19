namespace exam_system.Features.Quizzes.AdminCreateQuiz.Requests;

public class AdminCreateQuizRequest
{
    public Guid DiplomaId { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public int DurationMinutes { get; set; }
    public int PassScore { get; set; } = 60;
    public int? MaxAttempts { get; set; }
}

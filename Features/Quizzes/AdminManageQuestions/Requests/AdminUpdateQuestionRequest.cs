namespace exam_system.Features.Quizzes.AdminManageQuestions.Requests;

public sealed class AdminUpdateQuestionRequest
{
    public string? Text { get; set; }
    public string? Explanation { get; set; }
    public int? OrderIndex { get; set; }
}

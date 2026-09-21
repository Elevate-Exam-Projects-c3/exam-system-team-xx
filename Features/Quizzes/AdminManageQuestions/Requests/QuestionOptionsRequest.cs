namespace exam_system.Features.Quizzes.AdminManageQuestions.Requests;

public sealed class QuestionOptionsRequest
{
    public string OptionText { get; set; } = default!;
    public bool IsCorrect { get; set; }
}

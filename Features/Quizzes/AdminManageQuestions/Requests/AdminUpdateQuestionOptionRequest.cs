namespace exam_system.Features.Quizzes.AdminManageQuestions.Requests;

public sealed class AdminUpdateQuestionOptionRequest
{
    public string? OptionText { get; set; }
    public bool? IsCorrect { get; set; }
}

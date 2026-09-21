namespace exam_system.Features.Quizzes.AdminManageQuestions.Requests;

public sealed class AdminUpdateQuestionWithOptionsRequest
{
    public string? Text { get; set; }
    public string? Explanation { get; set; }
    public int? OrderIndex { get; set; }
    public ICollection<AdminAddQuestionOptionRequest>? Options { get; set; }
}

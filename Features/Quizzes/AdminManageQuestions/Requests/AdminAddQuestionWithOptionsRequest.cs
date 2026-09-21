using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Quizzes;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Requests;

public sealed class AdminAddQuestionWithOptionsRequest
{
    public Guid QuizId { get; set; }
    public string Text { get; set; } = default!;
    public string? Explanation { get; set; }
    public int OrderIndex { get; set; }
    public ICollection<QuestionOptionsRequest> Options { get; set; } = [];
}

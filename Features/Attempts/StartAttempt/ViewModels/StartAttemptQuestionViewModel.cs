namespace exam_system.Features.Attempts.StartAttempt.ViewModels;

public sealed class StartAttemptQuestionViewModel
{
    public string QuestionText { get; set; } = default!;
    public ICollection<string> QuestionOptions { get; set; } = [];
}

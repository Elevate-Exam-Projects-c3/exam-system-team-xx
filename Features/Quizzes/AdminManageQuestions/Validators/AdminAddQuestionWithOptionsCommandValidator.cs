using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public sealed class AdminAddQuestionWithOptionsCommandValidator : AbstractValidator<AdminAddQuestionWithOptionsCommand>
{
    public AdminAddQuestionWithOptionsCommandValidator()
    {
        RuleFor(x => x.AddQuestionWithOptionsRequest.Text)
            .NotEmpty().WithMessage("Question text is mandatory and shall not be empty.")
            .MinimumLength(2).WithMessage("Qustion text must be at least 2 characters.");

        RuleFor(x => x.AddQuestionWithOptionsRequest.OrderIndex)
            .GreaterThan(0).WithMessage("Order index shall not be less than 1.");

        RuleFor(x => x.AddQuestionWithOptionsRequest.Options)
            .Must(options => options.Count >= 2).WithMessage("Question must have at least 2 options.")
            .Must(options => options.Count(option => option.IsCorrect is true) == 1).WithMessage("Question mustn't have more than one correct option.")
            .Must(options => options.All(option => !string.IsNullOrWhiteSpace(option.OptionText))).WithMessage("All question options must have option text.");
    }
}

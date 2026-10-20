using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public sealed class AdminUpdateQuestionOptionCommandValidator : AbstractValidator<AdminUpdateQuestionOptionCommand>
{
    public AdminUpdateQuestionOptionCommandValidator()
    {
        RuleFor(x => x.OptionText)
            .NotEmpty().WithMessage("Question option must have option text.");
    }
}

using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public sealed class AdminAddQuestionOptionCommandValidator : AbstractValidator<AdminAddQuestionOptionCommand>
{
    public AdminAddQuestionOptionCommandValidator()
    {
        RuleFor(x => x.AdminAddQuestionOptionRequest.OptionText)
            .NotEmpty().WithMessage("A question option must have an option text.");

        RuleFor(x => x.AdminAddQuestionOptionRequest.IsCorrect)
            .Equal(false).WithMessage("Question already has a correct option.");
    }
}

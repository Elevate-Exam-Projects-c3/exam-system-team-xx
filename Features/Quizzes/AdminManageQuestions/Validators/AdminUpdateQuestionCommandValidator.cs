using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public sealed class AdminUpdateQuestionCommandValidator : AbstractValidator<AdminUpdateQuestionCommand>
{
    public AdminUpdateQuestionCommandValidator()
    {
        RuleFor(x => x.AdminUpdateQuestionRequest.Text)
        .NotEqual(string.Empty).WithMessage("Question text is mandatory and shall not be empty.")
        .MinimumLength(2).WithMessage("Qustion text must be at least 2 characters.");

        RuleFor(x => x.AdminUpdateQuestionRequest.OrderIndex)
            .GreaterThan(0).WithMessage("Order index shall not be less than 1.");
    }
}

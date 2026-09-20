using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Validators;

public sealed class AdminUpdateQuizCommandValidator : AbstractValidator<AdminUpdateQuizCommand>
{
    public AdminUpdateQuizCommandValidator()
    {
        RuleFor(x => x.AdminUpdateQuizRequest.Title)
            .NotEqual(string.Empty).WithMessage("Quiz title shall not be empty and must be from 3 to 200 characters.")
            .MinimumLength(3).WithMessage("Quiz title minimum length shall be 2 characters.")
            .MaximumLength(200).WithMessage("Quiz title maximum length shall be 200 characters.");

        RuleFor(x => x.AdminUpdateQuizRequest.DurationMinutes)
            .GreaterThan(0).WithMessage("Quiz duration minutes shall be greater than 0 (Positive Value).");

        RuleFor(x => x.AdminUpdateQuizRequest.PassScore)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Quiz pass score must be from 1 - 100.");
    }
}

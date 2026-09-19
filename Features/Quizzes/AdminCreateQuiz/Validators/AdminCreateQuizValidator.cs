using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Validators;

public sealed class AdminCreateQuizValidator : AbstractValidator<AdminCreateQuizCommand>
{
    public AdminCreateQuizValidator()
    {
        RuleFor(x => x.AdminCreateQuizRequest.Title)
            .NotNull().WithMessage("Quiz must have a title.")
            .NotEmpty().WithMessage("Quiz title shall not be empty and must be from 3 to 200 characters.")
            .MinimumLength(3).WithMessage("Quiz title minimum length shall be 2 characters.")
            .MaximumLength(200).WithMessage("Quiz title maximum length shall be 200 characters.");

        RuleFor(x => x.AdminCreateQuizRequest.DurationMinutes)
            .GreaterThan(0).WithMessage("Quiz duration minutes shall be greater than 0 (Positive Value).");

        RuleFor(x => x.AdminCreateQuizRequest.PassScore)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Quiz pass score must be from 1 - 100.");
    }
}

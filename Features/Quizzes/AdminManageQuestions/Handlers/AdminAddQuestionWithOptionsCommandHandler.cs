using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminAddQuestionWithOptionsCommandHandler : IRequestHandler<AdminAddQuestionWithOptionsCommand, Result>
{
    private readonly IGenericRepository<Question> _questionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminAddQuestionWithOptionsCommandHandler(IGenericRepository<Question> questionRepo, IUnitOfWork unitOfWork)
    {
        _questionRepo = questionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminAddQuestionWithOptionsCommand request, CancellationToken cancellationToken)
    {
        // Check whether the new question to be created has the same orderindex
        // as other existing question under the same quiz
        if (await _questionRepo.AnyAsync(q => q.QuizId == request.AdminAddQuestionWithOptionsRequest.QuizId &&
            q.OrderIndex == request.AdminAddQuestionWithOptionsRequest.OrderIndex, cancellationToken))
            return Result.Failure(Error.Validation("Question.ExistingOrderIndex", "Order index is already assigned to other existing question under the same quiz."));

        // Create new question with options
        var question = new Question()
        {
            QuizId = request.AdminAddQuestionWithOptionsRequest.QuizId,
            Text = request.AdminAddQuestionWithOptionsRequest.Text.Trim(),
            Explanation = request.AdminAddQuestionWithOptionsRequest.Explanation?.Trim(),
            OrderIndex = request.AdminAddQuestionWithOptionsRequest.OrderIndex,
            Options = request.AdminAddQuestionWithOptionsRequest.Options.Select(option => 
                new QuestionOption()
                {
                    QuestionId = request.AdminAddQuestionWithOptionsRequest.QuizId,
                    OptionText = option.OptionText.Trim(),
                    IsCorrect = option.IsCorrect,
                }
            ).ToList(),
        };
        _questionRepo.Add(question);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

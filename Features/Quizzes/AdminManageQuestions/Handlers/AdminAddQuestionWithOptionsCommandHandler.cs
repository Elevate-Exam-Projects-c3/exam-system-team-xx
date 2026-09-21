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
        if (await _questionRepo.AnyAsync(
            q => q.QuizId == request.AddQuestionWithOptionsRequest.QuizId &&
            q.OrderIndex == request.AddQuestionWithOptionsRequest.OrderIndex))
            return Result.Failure(Error.Validation("Question.ExistingOrderIndex", "Order index is already assigned to other existing question under the same quiz."));

        // Create new question with options
        var question = new Question()
        {
            QuizId = request.AddQuestionWithOptionsRequest.QuizId,
            Text = request.AddQuestionWithOptionsRequest.Text,
            Explanation = request.AddQuestionWithOptionsRequest.Explanation,
            OrderIndex = request.AddQuestionWithOptionsRequest.OrderIndex,
            Options = request.AddQuestionWithOptionsRequest.Options.Select(option => 
                new QuestionOption()
                {
                    QuestionId = request.AddQuestionWithOptionsRequest.QuizId,
                    OptionText = option.OptionText,
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

using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminAddQuestionOptionCommandHandler : IRequestHandler<AdminAddQuestionOptionCommand, Result>
{
    private readonly IGenericRepository<QuestionOption> _questionOptionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminAddQuestionOptionCommandHandler(IGenericRepository<QuestionOption> questionOptionRepo, IUnitOfWork unitOfWork)
    {
        _questionOptionRepo = questionOptionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminAddQuestionOptionCommand request, CancellationToken cancellationToken)
    {
        // Create new question option under a specific question
        var newQuestionOption = new QuestionOption()
        {
            QuestionId = request.QuestionId,
            OptionText = request.AdminAddQuestionOptionRequest.OptionText,
            IsCorrect = request.AdminAddQuestionOptionRequest.IsCorrect
        };
        _questionOptionRepo.Add(newQuestionOption);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

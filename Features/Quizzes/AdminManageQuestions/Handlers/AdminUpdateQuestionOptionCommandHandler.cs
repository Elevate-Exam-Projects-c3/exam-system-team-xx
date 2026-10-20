using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminUpdateQuestionOptionCommandHandler : IRequestHandler<AdminUpdateQuestionOptionCommand, Result>
{
    private readonly IGenericRepository<QuestionOption> _questionOptionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminUpdateQuestionOptionCommandHandler(IGenericRepository<QuestionOption> questionOptionRepo, IUnitOfWork unitOfWork)
    {
        _questionOptionRepo = questionOptionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminUpdateQuestionOptionCommand request, CancellationToken cancellationToken)
    {
        // Get the question option to be updated
        var questionOptionToBeUpdated = await _questionOptionRepo.GetByIdAsync(request.OptionId, cancellationToken);

        // Check whether returned question option is null
        if (questionOptionToBeUpdated is null)
            return Result.Failure(error: Error.NotFound("QuestionOption.NotFound", $"Question option of Id ({request.OptionId}) doesn't exist on the system."));
    
        // Update the option
        questionOptionToBeUpdated.OptionText = request.OptionText.Trim();
        _questionOptionRepo.Update(questionOptionToBeUpdated);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminSetCorrectQuestionOptionCommandHandler : IRequestHandler<AdminSetCorrectQuestionOptionCommand, Result>
{
    private readonly IGenericRepository<QuestionOption> _questionOptionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminSetCorrectQuestionOptionCommandHandler(IGenericRepository<QuestionOption> questionOptionRepo, IUnitOfWork unitOfWork)
    {
        _questionOptionRepo = questionOptionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminSetCorrectQuestionOptionCommand request, CancellationToken cancellationToken)
    {
        // Get the question option
        var questionOptionToBeSetAsCorrect = await _questionOptionRepo.GetByIdAsync(request.OptionId, cancellationToken);

        // Check whether returned question option is null
        if (questionOptionToBeSetAsCorrect is null)
            return Result.Failure(error: Error.NotFound("QuestionOption.NotFound", $"Question option of Id ({request.OptionId}) doesn't exist on the system."));

        // Check whether this question option is already correct
        if(!questionOptionToBeSetAsCorrect.IsCorrect)
        {
            // Toggle the IsCorrect property of already existing correct option to ensure 
            // one correct option under the same question
            var currentCorrectQuestionOption = await _questionOptionRepo.FirstOrDefaultAsync(option => option.IsCorrect == true && option.QuestionId == request.QuestionId, cancellationToken);
            currentCorrectQuestionOption!.IsCorrect = false;

            // Update the question option
            _questionOptionRepo.Update(currentCorrectQuestionOption);

            // Set IsCorrect property of this question option to true
            questionOptionToBeSetAsCorrect.IsCorrect = true;

            // Update the question option
            _questionOptionRepo.Update(questionOptionToBeSetAsCorrect);

            // Save changes to DB
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}

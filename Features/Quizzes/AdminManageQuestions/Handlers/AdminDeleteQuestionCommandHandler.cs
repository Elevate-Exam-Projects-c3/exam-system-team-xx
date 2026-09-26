using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminDeleteQuestionCommandHandler : IRequestHandler<AdminDeleteQuestionCommand, Result>
{
    private readonly IGenericRepository<Question> _questionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminDeleteQuestionCommandHandler(IGenericRepository<Question> quesstionRepo, IUnitOfWork unitOfWork)
    {
        _questionRepo = quesstionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminDeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        // Get the question to be deleted
        var questionToBeDeleted = await _questionRepo.GetByIdAsync(request.QuestionId, cancellationToken);
        
        // Check if returned question is null
        if (questionToBeDeleted is null)
            return Result.Failure(error: Error.NotFound("Question.NotFound", $"Question of Id ({request.QuestionId}) doesn't exist on the system."));

        // Delete the question
        _questionRepo.Delete(questionToBeDeleted);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

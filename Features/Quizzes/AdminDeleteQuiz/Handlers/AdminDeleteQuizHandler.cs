using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Handlers;

public sealed class AdminDeleteQuizHandler : IRequestHandler<AdminDeleteQuizCommand, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminDeleteQuizHandler(IGenericRepository<Quiz> quizRepo, IUnitOfWork unitOfWork)
    {
        _quizRepo = quizRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminDeleteQuizCommand request, CancellationToken cancellationToken)
    {
        // Get the quiz to be deleted
        var quizToBeDeleted = await _quizRepo.GetByIdAsync(request.QuizId, cancellationToken);

        // Check if returned quiz is null
        if(quizToBeDeleted is null)
            return Result.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        // Check whether the quiz is published
        if(quizToBeDeleted.Status is QuizStatus.Published)
            return Result.Failure(error: Error.Conflict("Quiz.PublishedCannotDelete", $"Quiz of Id ({request.QuizId}) is already published and needs to be unpublished first to be deleted."));

        // Delete the quiz
        _quizRepo.Delete(quizToBeDeleted);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

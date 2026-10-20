using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Handlers;

public sealed class AdminUnpublishQuizCommandHandler : IRequestHandler<AdminUnpublishQuizCommand, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminUnpublishQuizCommandHandler(IGenericRepository<Quiz> quizRepo, IUnitOfWork unitOfWork)
    {
        _quizRepo = quizRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminUnpublishQuizCommand request, CancellationToken cancellationToken)
    {
        // Get the published quiz to be unpublished
        var quizToBeUnpublished = await _quizRepo.GetAll()
            .Include(quiz => quiz.Attempts)
            .FirstOrDefaultAsync(cancellationToken);

        // Check if returned quiz is null
        if(quizToBeUnpublished is null)
            return Result.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        // Check whether quiz is already published
        if (quizToBeUnpublished.Status != QuizStatus.Published)
            return Result.Failure(error: Error.Conflict("Quiz.NotPublished", $"Quiz of Id ({request.QuizId}) is not published yet."));

        // Check whether quiz has already in progress attempts
        if(quizToBeUnpublished.Attempts.Count(attempt => attempt.Status == AttemptStatus.InProgress) > 0)
            return Result.Failure(error: Error.Conflict("Quiz.InProgressAttempts", $"Quiz of Id ({request.QuizId}) cannot be unpublished due to existing in-progress quiz attempts."));

        // Unpublish the quiz
        quizToBeUnpublished.Status = QuizStatus.Draft;
        _quizRepo.Update(quizToBeUnpublished);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
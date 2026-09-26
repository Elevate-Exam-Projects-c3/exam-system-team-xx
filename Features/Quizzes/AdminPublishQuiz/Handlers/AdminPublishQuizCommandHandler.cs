using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers;

public sealed class AdminPublishQuizCommandHandler : IRequestHandler<AdminPublishQuizCommand, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AdminPublishQuizCommandHandler(IGenericRepository<Quiz> quizRepo, IUnitOfWork unitOfWork)
    {
        _quizRepo = quizRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AdminPublishQuizCommand request, CancellationToken cancellationToken)
    {
        // Get the quiz to be published
        var quizToBePublished = await _quizRepo.GetByIdAsync(request.QuizId, cancellationToken);

        // Check if the returned quiz is null
        if(quizToBePublished is null)
            return Result.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));
        
        // Set quiz status to Published
        quizToBePublished.Status = QuizStatus.Published;

        // Set quiz PublishAt date to today
        quizToBePublished.PublishedAt = DateTime.UtcNow;

        // Udpate the quiz
        _quizRepo.Update(quizToBePublished);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

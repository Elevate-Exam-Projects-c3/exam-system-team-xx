using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Handlers;

public sealed class CheckQuizNotPublishedQueryHandler : IRequestHandler<CheckQuizNotPublishedQuery, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;

    public CheckQuizNotPublishedQueryHandler(IGenericRepository<Quiz> quizRepo)
    {
        _quizRepo = quizRepo;
    }

    public async Task<Result> Handle(CheckQuizNotPublishedQuery request, CancellationToken cancellationToken)
    {
        // Check if quiz whose Id is passed exists
        var quizExists = await _quizRepo.AnyAsync(d => d.Id == request.QuizId, cancellationToken);

        if (!quizExists)
            return Result.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        // Check if quiz status is not published
        var quizPublished = await _quizRepo.AnyAsync(d => d.Id == request.QuizId && d.Status == QuizStatus.Published, cancellationToken);
        
        if(quizPublished)
            return Result.Failure(error: Error.Conflict("Quiz.Published", $"Quiz of Id ({request.QuizId}) is currently published on the system."));

        return Result.Success();
    }
}

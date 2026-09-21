using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Handlers;

public sealed class CheckQuizExistsQueryHandler : IRequestHandler<CheckQuizExistsQuery, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;

    public CheckQuizExistsQueryHandler(IGenericRepository<Quiz> quizRepo)
    {
        _quizRepo = quizRepo;
    }

    public async Task<Result> Handle(CheckQuizExistsQuery request, CancellationToken cancellationToken)
    {
        // Check if quiz whose Id is passed exists
        var quizExists = await _quizRepo.AnyAsync(d => d.Id == request.QuizId);

        if (!quizExists)
            return Result.Failure(error: Error.NotFound("Quiz.NotExist", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        return Result.Success();
    }
}

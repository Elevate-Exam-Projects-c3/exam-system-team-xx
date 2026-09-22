using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Handlers;

public sealed class CheckQuestionExistsQueryHandler : IRequestHandler<CheckQuestionExistsQuery, Result>
{
    private readonly IGenericRepository<Question> _questionRepo;

    public CheckQuestionExistsQueryHandler(IGenericRepository<Question> questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public async Task<Result> Handle(CheckQuestionExistsQuery request, CancellationToken cancellationToken)
    {
        // Check if question whose Id is passed exists
        var questionExists = await _questionRepo.AnyAsync(d => d.Id == request.QuestionId, cancellationToken);

        if (!questionExists)
            return Result.Failure(error: Error.NotFound("Question.NotFound", $"Question of Id ({request.QuestionId}) doesn't exist on the system."));

        return Result.Success();
    }
}
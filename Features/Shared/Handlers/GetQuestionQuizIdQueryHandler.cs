using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Handlers;

public sealed class GetQuestionQuizIdQueryHandler : IRequestHandler<GetQuestionQuizIdQuery, Result<Guid>>
{
    private readonly IGenericRepository<Question> _questionRepo;

    public GetQuestionQuizIdQueryHandler(IGenericRepository<Question> questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public async Task<Result<Guid>> Handle(GetQuestionQuizIdQuery request, CancellationToken cancellationToken)
    {
        // Get the question
        var question = await _questionRepo.GetByIdAsync(request.QuestionId, cancellationToken);

        // Check if returned question is null
        if (question is null)
            return Result<Guid>.Failure(error: Error.NotFound("Question.NotFound", $"Question of Id ({request.QuestionId}) doesn't exist on the system."));
        
        // Get the question quiz id 
        var quizId = question.QuizId;

        return Result<Guid>.Success(quizId);
    }
}

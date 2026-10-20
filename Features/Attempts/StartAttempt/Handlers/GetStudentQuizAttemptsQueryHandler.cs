using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public sealed class GetStudentQuizAttemptsQueryHandler : IRequestHandler<GetStudentQuizAttemptsQuery, int>
{
    private readonly IGenericRepository<QuizAttempt> _quizAttemptRepo;

    public GetStudentQuizAttemptsQueryHandler(IGenericRepository<QuizAttempt> quizAttemptRepo)
    {
        _quizAttemptRepo = quizAttemptRepo;
    }

    public async Task<int> Handle(GetStudentQuizAttemptsQuery request, CancellationToken cancellationToken)
    {
        // Get student attempts (submitted/timed_out attempts)
        var studentQuizAttempts = await _quizAttemptRepo
            .CountAsync(qa => qa.StudentId == request.StudentId && qa.QuizId == request.QuizId && qa.Status != AttemptStatus.InProgress, cancellationToken);

        return studentQuizAttempts;
    }
}

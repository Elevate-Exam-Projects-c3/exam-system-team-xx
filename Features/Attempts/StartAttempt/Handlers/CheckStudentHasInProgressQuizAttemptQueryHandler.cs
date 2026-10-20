using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public sealed class CheckStudentHasInProgressQuizAttemptQueryHandler : IRequestHandler<CheckStudentHasInProgressQuizAttemptQuery, Result>
{
    private readonly IGenericRepository<QuizAttempt> _quizAttemptRepo;

    public CheckStudentHasInProgressQuizAttemptQueryHandler(IGenericRepository<QuizAttempt> quizAttemptRepo)
    {
        _quizAttemptRepo = quizAttemptRepo;
    }

    public async Task<Result> Handle(CheckStudentHasInProgressQuizAttemptQuery request, CancellationToken cancellationToken)
    {
        // Check whether student has an already existing In-Progress quiz attempt
        var inProgressQuizAttemptId = await _quizAttemptRepo
            .Get(qa => qa.StudentId == request.StudentId && qa.QuizId == request.QuizId && qa.Status == AttemptStatus.InProgress)
            .Select(qa => qa.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (inProgressQuizAttemptId != Guid.Empty)
            return Result.Failure(Error.Conflict("QuizAttempt.AlreadyInProgress",$"Quiz attempt whose Id is ({inProgressQuizAttemptId}) is already In-Progress."));

        return Result.Success();
    }
}

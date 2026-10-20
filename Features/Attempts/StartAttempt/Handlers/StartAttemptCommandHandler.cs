using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public sealed class StartAttemptCommandHandler : IRequestHandler<StartAttemptCommand, Result>
{
    private readonly IGenericRepository<QuizAttempt> _quizAttemptRepo;
    private readonly IUnitOfWork _unitOfWork;

    public StartAttemptCommandHandler(IGenericRepository<QuizAttempt> quizAttemptRepo, IUnitOfWork unitOfWork)
    {
        _quizAttemptRepo = quizAttemptRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(StartAttemptCommand request, CancellationToken cancellationToken)
    {
        // Create and start a new quiz attempt
        var quizAttemptStartTime = DateTime.UtcNow;
        var quizAttempt = new QuizAttempt()
        {
            Status = AttemptStatus.InProgress,
            StartTime = quizAttemptStartTime,
            Deadline = quizAttemptStartTime.AddMinutes(request.QuizDurationInMinutes),
            StudentId = request.StudentId,
            QuizId = request.QuizId
        };
        _quizAttemptRepo.Add(quizAttempt);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

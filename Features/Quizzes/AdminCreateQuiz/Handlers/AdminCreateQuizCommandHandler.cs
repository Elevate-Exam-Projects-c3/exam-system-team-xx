using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Persistence.DataAccess;
using exam_system.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Handlers;

public class AdminCreateQuizCommandHandler : IRequestHandler<AdminCreateQuizCommand, Result<Unit>>
{
    private readonly IGenericRepository<Quiz, Guid> _quizRepo;

    public AdminCreateQuizCommandHandler(IGenericRepository<Quiz, Guid> quizRepo)
    {
        _quizRepo = quizRepo;
    }

    public async Task<Result<Unit>> Handle(AdminCreateQuizCommand request, CancellationToken cancellationToken)
    {
        // Add new quiz under a specific diploma
        var newQuiz = new Quiz()
        {
            DiplomaId = request.AdminCreateQuizRequest.DiplomaId,
            Title = request.AdminCreateQuizRequest.Title,
            Instructions = request.AdminCreateQuizRequest.Instructions,
            DurationMinutes = request.AdminCreateQuizRequest.DurationMinutes,
            PassScore = request.AdminCreateQuizRequest.PassScore,
            MaxAttempts = request.AdminCreateQuizRequest.MaxAttempts,
        };
        _quizRepo.Add(newQuiz);

        return Result.Success(Unit.Value);
    }
}

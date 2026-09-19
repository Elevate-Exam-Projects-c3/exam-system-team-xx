using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Handlers;

public sealed class AdminCreateQuizCommandHandler : IRequestHandler<AdminCreateQuizCommand, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;

    public AdminCreateQuizCommandHandler(IGenericRepository<Quiz> quizRepo)
    {
        _quizRepo = quizRepo;
    }

    public async Task<Result> Handle(AdminCreateQuizCommand request, CancellationToken cancellationToken)
    {
        // Add new quiz under a specific diploma
        var newQuiz = new Quiz()
        {
            DiplomaId = request.AdminCreateQuizRequest.DiplomaId,
            Title = request.AdminCreateQuizRequest.Title.Trim(),
            Instructions = request.AdminCreateQuizRequest.Instructions?.Trim(),
            DurationMinutes = request.AdminCreateQuizRequest.DurationMinutes,
            PassScore = request.AdminCreateQuizRequest.PassScore,
            MaxAttempts = request.AdminCreateQuizRequest.MaxAttempts,
        };
        _quizRepo.Add(newQuiz);

        return Result.Success();
    }
}

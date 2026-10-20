using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Shared.Handlers;

public sealed class GetQuizToStartNewAttemptQueryHandler : IRequestHandler<GetQuizToStartNewAttemptQuery, Result<StartAttemptQuizResponse>>
{
    private readonly IGenericRepository<Quiz> _quizRepo;

    public GetQuizToStartNewAttemptQueryHandler(IGenericRepository<Quiz> quizRepo)
    {
        _quizRepo = quizRepo;
    }

    public async Task<Result<StartAttemptQuizResponse>> Handle(GetQuizToStartNewAttemptQuery request, CancellationToken cancellationToken)
    {
        // Get quiz to start a new attempt
        var quizToStartNewAttempt = await _quizRepo
            .Get(quiz => quiz.Id == request.QuizId)
            .Select(quiz => new StartAttemptQuizResponse()
            {
                DiplomaId = quiz.DiplomaId,
                QuizTitle = quiz.Title,
                QuizDurationInMinutes = quiz.DurationMinutes,
                QuizInstructions = quiz.Instructions,
                QuizMaxAttempts = quiz.MaxAttempts,
                QuizStatus = quiz.Status,
                QuizPassScore = quiz.PassScore,
                Questions = quiz.Questions.Select(question => new StartAttemptQuestionResponse()
                {
                    QuestionText = question.Text,
                    QuestionOptions = question.Options.Select(option => option.OptionText).ToList()
                }).ToList()
                
            }).FirstOrDefaultAsync(cancellationToken);

        // Check if quiz exits
        if (quizToStartNewAttempt is null)
            return Result<StartAttemptQuizResponse>.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        // Check if quiz is already published
        if (quizToStartNewAttempt.QuizStatus is not QuizStatus.Published)
            return Result<StartAttemptQuizResponse>.Failure(error: Error.Conflict("Quiz.NotPublished", $"Quiz of Id ({request.QuizId}) is currently not published on the system."));

        return Result<StartAttemptQuizResponse>.Success(quizToStartNewAttempt);
    }
}

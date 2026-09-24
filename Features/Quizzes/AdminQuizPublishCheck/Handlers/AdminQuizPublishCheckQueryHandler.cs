using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Handlers;

public sealed class AdminQuizPublishCheckQueryHandler : IRequestHandler<AdminQuizPublishCheckQuery, Result<IReadOnlyList<ChecklistItemResponse>>>
{
    private readonly IGenericRepository<Quiz> _quizRepo;

    public AdminQuizPublishCheckQueryHandler(IGenericRepository<Quiz> quizRepo)
    {
        _quizRepo = quizRepo;
    }

    public async Task<Result<IReadOnlyList<ChecklistItemResponse>>> Handle(AdminQuizPublishCheckQuery request, CancellationToken cancellationToken)
    {
        // Get quiz checklist query
        var checklistQuery = await _quizRepo.GetAll()
            .Where(quiz => request.QuizId == quiz.Id)
            .Select(quiz => new
            {
                QuizHasAtLeastOneQuestion = quiz.Questions.Count() > 0,
                QuestionsOptions = quiz.Questions.Select(question => question.Options),
                QuestionDurationMinutesValid = quiz.DurationMinutes > 0,
                QuestionPassScoreWithinValidRange = quiz.PassScore > 0 && quiz.PassScore <= 100
            })
            .FirstOrDefaultAsync(cancellationToken);

        // Check whether quiz already exists
        if(checklistQuery is null)
            return Result<IReadOnlyList<ChecklistItemResponse>>.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        // Populate checklist
        List<ChecklistItemResponse> ckecklist = [];

        // Check if quiz has at least on question exists
        ckecklist.Add(new ChecklistItemResponse()
        {
            Name = "Quiz must have at least one question.",
            Success = checklistQuery.QuizHasAtLeastOneQuestion,
            ErrorMessage = checklistQuery.QuizHasAtLeastOneQuestion ? null : "Quiz doesn't have questions yet."
        });

        // Check if question pass score is within valid range
        ckecklist.Add(new ChecklistItemResponse()
        {
            Name = "Quiz pass score must be within 0-100.",
            Success = checklistQuery.QuestionPassScoreWithinValidRange,
            ErrorMessage = checklistQuery.QuestionPassScoreWithinValidRange ? null : "Quiz pass score is not within the valid range."
        });
            
        // Check if question has a valid duration minutes value
        ckecklist.Add(new ChecklistItemResponse()
        {
            Name = "Quiz duration minutes value must be valid (Postive Number).",
            Success = checklistQuery.QuestionDurationMinutesValid,
            ErrorMessage = checklistQuery.QuestionDurationMinutesValid ? null : "Quiz duration minutes valid is invalid."
        });

        // Check that every question has exactly one correct option marked
        var EveryQuestionHasCorrectOption = true;

        foreach (var questionOptions in checklistQuery.QuestionsOptions)
        {
            if(questionOptions.SingleOrDefault(qo => qo.IsCorrect == true) is null)
            {
                EveryQuestionHasCorrectOption = false;
                break;
            }
        }

        ckecklist.Add(new ChecklistItemResponse()
        {
            Name = "Every question must have exactly one correct option marked.",
            Success = EveryQuestionHasCorrectOption,
            ErrorMessage = EveryQuestionHasCorrectOption ? null : "One or more question(s) don't have exactly one correct option marked."
        });

        return Result<IReadOnlyList<ChecklistItemResponse>>.Success(ckecklist);
    }
}

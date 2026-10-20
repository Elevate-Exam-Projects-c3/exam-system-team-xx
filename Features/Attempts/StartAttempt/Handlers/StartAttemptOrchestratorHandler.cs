using exam_system.Extensions;
using exam_system.Features.Attempts.StartAttempt.Commands;
using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Attempts.StartAttempt.Orchestrators;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public sealed class StartAttemptOrchestratorHandler : IRequestHandler<StartAttemptOrchestrator, Result<StartAttemptQuizResponse>>
{
    private readonly IMediator _mediator;

    public StartAttemptOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<StartAttemptQuizResponse>> Handle(StartAttemptOrchestrator request, CancellationToken cancellationToken)
    {
        // Get Id of student associated with logged-in user
        var studentIdResult = await _mediator.Send(new GetStudentIdQuery(request.UserId), cancellationToken);
        if (studentIdResult.IsFailure)
            return Result<StartAttemptQuizResponse>.Failure(studentIdResult.Errors[0]);
        var studentId = studentIdResult.Value;

        // Get published quiz to start a new attempt
        var quizToStartNewAttemptResult = await _mediator.Send(new GetQuizToStartNewAttemptQuery(request.QuizId), cancellationToken);
        if (quizToStartNewAttemptResult.IsFailure)
            return Result<StartAttemptQuizResponse>.Failure(quizToStartNewAttemptResult.Errors[0]);
        var quiz = quizToStartNewAttemptResult.Value;

        // Check whether the student is enrolled in the diploma associated with the quiz
        var isStudentEnrolledInDiploma = await _mediator.Send(new CheckStudentIsEnrolledInDiplomaQuery(studentId, quiz.DiplomaId), cancellationToken);
        if(isStudentEnrolledInDiploma.IsFailure)
            return Result<StartAttemptQuizResponse>.Failure(isStudentEnrolledInDiploma.Errors[0]);

        // Check whether student has already in-progress quiz attempt
        var studentHasInProgressQuizAttemptResult = await _mediator.Send(new CheckStudentHasInProgressQuizAttemptQuery(studentId, request.QuizId), cancellationToken);
        if(studentHasInProgressQuizAttemptResult.IsFailure)
            return Result<StartAttemptQuizResponse>.Failure(studentHasInProgressQuizAttemptResult.Errors[0]);

        // Check whether student has reached maximum attempts (submitted/timed_out attempts)
        var studentQuizAttempts = await _mediator.Send(new GetStudentQuizAttemptsQuery(studentId, request.QuizId), cancellationToken);
        if (studentQuizAttempts >= quiz.QuizMaxAttempts)
            return Result<StartAttemptQuizResponse>.Failure(Error.Conflict("QuizAttempt.ReachedMaximumAttempts","Quiz attempts exhausted."));

        // Create and start a new quiz attempt
        await _mediator.Send(new StartAttemptCommand(studentId, request.QuizId, quiz.QuizDurationInMinutes), cancellationToken);
        quiz.StudentQuizAttempts = studentQuizAttempts + 1;

        // Shuffle the set of quiz questions
        quiz.Questions = quiz.Questions.ShuffleList();

        // Shuffle the options of each question
        foreach(var question in quiz.Questions)
            question.QuestionOptions = question.QuestionOptions.ShuffleList();

        return Result<StartAttemptQuizResponse>.Success(quiz);
    }
}

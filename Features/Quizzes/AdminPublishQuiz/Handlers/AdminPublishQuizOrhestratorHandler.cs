using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers;

public sealed class AdminPublishQuizOrhestratorHandler : IRequestHandler<AdminPublishQuizOrhestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminPublishQuizOrhestratorHandler(IMediator mediator)
    {
        this._mediator = mediator;
    }

    public async Task<Result> Handle(AdminPublishQuizOrhestrator request, CancellationToken cancellationToken)
    {
        // Check quiz readiness to publish
        var quizPublishReadinessCheckResult = await _mediator.Send(new AdminQuizPublishCheckQuery(request.QuizId), cancellationToken);

        if (quizPublishReadinessCheckResult.IsFailure)
            return quizPublishReadinessCheckResult;

        if(quizPublishReadinessCheckResult.Value.Any(item => item.Passed == false))
        {
            var checklistErrorMessages = 
                quizPublishReadinessCheckResult.Value
                .Where(checklistItem => checklistItem.Passed == false)
                .Select(checklistItem => checklistItem.ErrorMessage)
                .ToList();

            var checklistErrors = checklistErrorMessages.Select(errorMsg => Error.Validation("Quiz.Pre-PublishChecklist", errorMsg!));

            return Result.Failure(checklistErrors);
        } 
        
        // Publish the quiz
        var quizPublishResult = await _mediator.Send(new AdminPublishQuizCommand(request.QuizId), cancellationToken);

        return Result.Success();
    }
}

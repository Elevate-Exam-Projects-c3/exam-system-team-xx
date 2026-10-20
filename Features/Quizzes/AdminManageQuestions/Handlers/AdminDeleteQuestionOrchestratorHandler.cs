using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminDeleteQuestionOrchestratorHandler : IRequestHandler<AdminDeleteQuestionOrchestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminDeleteQuestionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> Handle(AdminDeleteQuestionOrchestrator request, CancellationToken cancellationToken)
    {
        // Step 1: Get the question quiz id
        var quizIdResult = await _mediator.Send(new GetQuestionQuizIdQuery(request.QuestionId), cancellationToken);
        if (quizIdResult.IsFailure)
            return Result.Failure(quizIdResult.Errors[0]);

        var quizId = quizIdResult.Value;

        // Step 2: Check whether quiz status is published
        var quizStatusResult = await _mediator.Send(new CheckQuizNotPublishedQuery(quizId), cancellationToken);
        if (quizStatusResult.IsFailure)
            return quizStatusResult;

        // Step 3: Delete the question under the quiz
        var questionDeletedResult = await _mediator.Send(new AdminDeleteQuestionCommand(request.QuestionId), cancellationToken);
        if(questionDeletedResult.IsFailure)
            return questionDeletedResult;

        return Result.Success();
    }
}   

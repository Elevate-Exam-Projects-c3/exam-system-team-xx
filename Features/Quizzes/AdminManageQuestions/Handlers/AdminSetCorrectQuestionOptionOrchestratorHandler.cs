using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminSetCorrectQuestionOptionOrchestratorHandler : IRequestHandler<AdminSetCorrectQuestionOptionOrchestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminSetCorrectQuestionOptionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> Handle(AdminSetCorrectQuestionOptionOrchestrator request, CancellationToken cancellationToken)
    {
        // step 1: Check if passed question already exists
        var questionExistsResult = await _mediator.Send(new CheckQuestionExistsQuery(request.QuestionId), cancellationToken);
        if (questionExistsResult.IsFailure)
            return questionExistsResult;

        // step 2: Set the option under this question to correct
        var correctQuestionOptionResult = await _mediator.Send(new AdminSetCorrectQuestionOptionCommand(request.QuestionId, request.OptionId), cancellationToken);
        if (correctQuestionOptionResult.IsFailure)
            return correctQuestionOptionResult;

        return Result.Success();
    }
}

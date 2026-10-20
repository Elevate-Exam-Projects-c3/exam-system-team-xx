using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminAddQuestionOptionOrchestratorHandler : IRequestHandler<AdminAddQuestionOptionOrchestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminAddQuestionOptionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> Handle(AdminAddQuestionOptionOrchestrator request, CancellationToken cancellationToken)
    {
        // step 1: Check if passed question already exists
        var questionExistsResult = await _mediator.Send(new CheckQuestionExistsQuery(request.QuestionId), cancellationToken);
        if (questionExistsResult.IsFailure)
            return questionExistsResult;

        // step 2: Create the new option under this question
        var newQuestionOptionResult = await _mediator.Send(new AdminAddQuestionOptionCommand(request.QuestionId, request.AddQuestionOptionsRequest), cancellationToken);
        if (newQuestionOptionResult.IsFailure)
            return newQuestionOptionResult;

        return Result.Success();
    }
}

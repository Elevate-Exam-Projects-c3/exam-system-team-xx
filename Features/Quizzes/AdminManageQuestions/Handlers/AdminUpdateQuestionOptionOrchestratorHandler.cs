using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminUpdateQuestionOptionOrchestratorHandler : IRequestHandler<AdminUpdateQuestionOptionOrchestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminUpdateQuestionOptionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> Handle(AdminUpdateQuestionOptionOrchestrator request, CancellationToken cancellationToken)
    {
        // step 1: Check if passed question already exists
        var questionExistsResult = await _mediator.Send(new CheckQuestionExistsQuery(request.QuestionId), cancellationToken);
        if (questionExistsResult.IsFailure)
            return questionExistsResult;

        // step 2: Update the option under this question
        var updatedQuestionOptionResult = await _mediator.Send(new AdminUpdateQuestionOptionCommand(request.OptionId, request.OptionText), cancellationToken);
        if (updatedQuestionOptionResult.IsFailure)
            return updatedQuestionOptionResult;

        return Result.Success();
    }
}

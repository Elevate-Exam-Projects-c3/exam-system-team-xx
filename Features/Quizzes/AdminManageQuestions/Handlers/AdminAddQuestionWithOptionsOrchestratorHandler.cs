using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminAddQuestionWithOptionsOrchestratorHandler : IRequestHandler<AdminAddQuestionWithOptionsOrchestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminAddQuestionWithOptionsOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> Handle(AdminAddQuestionWithOptionsOrchestrator request, CancellationToken cancellationToken)
    {
        // step 1: Check if passed quiz already exists
        var quizExistsResult = await _mediator.Send(new CheckQuizExistsQuery(request.AdminAddQuestionWithOptionsRequest.QuizId), cancellationToken);
        if (quizExistsResult.IsFailure)
            return quizExistsResult;

        // step 2: Create the new question under this quiz
        var newQuestionResult = await _mediator.Send(new AdminAddQuestionWithOptionsCommand(request.AdminAddQuestionWithOptionsRequest), cancellationToken);
        if (newQuestionResult.IsFailure)
            return newQuestionResult;

        return Result.Success();
    }
}

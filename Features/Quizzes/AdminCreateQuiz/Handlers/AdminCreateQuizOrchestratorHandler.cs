using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Quizzes.AdminCreateQuiz.Orchestrators;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Handlers;

public sealed class AdminCreateQuizOrchestratorHandler : IRequestHandler<AdminCreateQuizOrchestrator, Result>
{
    private readonly IMediator _mediator;

    public AdminCreateQuizOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
    }

    public async Task<Result> Handle(AdminCreateQuizOrchestrator request, CancellationToken cancellationToken)
    {
        // step 1: Check if passed diploma already exists
        var diplomaExistsResult = await _mediator.Send(new CheckDiplomaExistsQuery(request.AdminCreateQuizRequest.DiplomaId));
        if (diplomaExistsResult.IsFailure)
            return diplomaExistsResult;

        // step 2: Create the new quiz under this diploma
        var newQuizResult = await _mediator.Send(new AdminCreateQuizCommand(request.AdminCreateQuizRequest),cancellationToken);
        if(newQuizResult.IsFailure)
            return newQuizResult;

        return Result.Success();
    }
}

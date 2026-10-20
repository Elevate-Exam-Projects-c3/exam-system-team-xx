using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Attempts.StartAttempt.Orchestrators;
using exam_system.Features.Attempts.StartAttempt.ViewModels;
using exam_system.Features.Shared;
using exam_system.Features.Shared.ResultPattern;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.StartAttempt.Controllers;

[Route("/api/quizzes")]
public sealed class StartAttemptController : BaseController
{
    private readonly IMapper _mapper;
    public StartAttemptController(IMediator mediator, IMapper mapper)
        : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("{quizId:guid}/attempts")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<StartAttemptQuizViewModel>>> StartQuizAttempt([FromRoute] Guid quizId, CancellationToken cancellationToken)
    {
        /*-----------------------Disclaimer: User Id here is fixed until Role-Based Access Control is implemented-----------------------*/

        var result = await _mediator.Send(new StartAttemptOrchestrator(new Guid("22222222-2222-2222-2222-222222222222"), quizId), cancellationToken);

        if (result.IsFailure)
            return FromResult(Result<StartAttemptQuizViewModel>.Failure(result.Errors[0]));

        var startAttemptQuizViewModel = _mapper.Map<StartAttemptQuizResponse, StartAttemptQuizViewModel>(result.Value);

        return FromResult(Result<StartAttemptQuizViewModel>.Success(startAttemptQuizViewModel), successCode: StatusCodes.Status201Created);
    }
}
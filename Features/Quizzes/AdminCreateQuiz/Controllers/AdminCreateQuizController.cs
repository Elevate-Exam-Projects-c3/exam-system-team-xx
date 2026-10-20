using exam_system.Features.Quizzes.AdminCreateQuiz.Orchestrators;
using exam_system.Features.Quizzes.AdminCreateQuiz.Requests;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers;

[Route("api/admin/quizzes")]
public sealed class AdminCreateQuizController : BaseController
{
    public AdminCreateQuizController(IMediator mediator):base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] AdminCreateQuizRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AdminCreateQuizOrchestrator(request), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "Quiz is created successfully !", successCode: StatusCodes.Status201Created);
    }
}

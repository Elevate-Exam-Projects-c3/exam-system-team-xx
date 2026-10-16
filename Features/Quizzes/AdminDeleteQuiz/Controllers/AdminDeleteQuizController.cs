using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Controllers;

[Route("api/admin/quizzes")]
public sealed class AdminDeleteQuizController : BaseController
{
    public AdminDeleteQuizController(IMediator mediator)
        : base(mediator)
    {
        
    }

    [HttpDelete("{quizId:Guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] Guid quizId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminDeleteQuizCommand(quizId), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "Quiz is deleted successfully from related diploma.", successCode: StatusCodes.Status200OK);
    }
}

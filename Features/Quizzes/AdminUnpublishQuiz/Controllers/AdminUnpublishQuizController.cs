using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Controllers;

[Route("api/admin/quizzes")]
public sealed class AdminUnpublishQuizController : BaseController
{
    public AdminUnpublishQuizController(IMediator mediator)
        :base(mediator)
    {
        
    }

    [HttpPatch("{quizId:Guid}/unpublish")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Unpublish([FromRoute] Guid quizId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminUnpublishQuizCommand(quizId), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "Quiz is unpublished successfully from related diploma.", successCode: StatusCodes.Status200OK);
    }
}

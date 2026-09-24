using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Controllers;

[Route("/api/admin/quizzes")]
public sealed class AdminQuizPublishCheckController : BaseController
{
    public AdminQuizPublishCheckController(IMediator mediator)
        :base(mediator)
    {
        
    }

    [HttpGet("{quizId:Guid}/publish-check")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ChecklistItemResponse>>>> GetPublishValidationChecklist([FromRoute] Guid quizId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminQuizPublishCheckQuery(quizId), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "Publish readiness checklist is completed.", StatusCodes.Status200OK);
    }
}

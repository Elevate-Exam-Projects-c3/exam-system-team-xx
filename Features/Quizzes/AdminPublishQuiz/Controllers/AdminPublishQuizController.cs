using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Controllers;

[Route("api/admin/quizzes")]
public sealed class AdminPublishQuizController : BaseController
{
    public AdminPublishQuizController(IMediator mediator)
        : base(mediator)
    {
        
    }

    [HttpPatch("{quizId:Guid}/publish")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> SetCorrectQuestionOption([FromRoute] Guid quizId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminPublishQuizOrhestrator(quizId), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "Quiz is published successfully on related diploma.", successCode: StatusCodes.Status200OK);
    }
}

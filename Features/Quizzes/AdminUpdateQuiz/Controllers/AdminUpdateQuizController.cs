using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Requests;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Controllers;

[Route("api/admin/quizzes")]
public sealed class AdminUpdateQuizController : BaseController
{
    public AdminUpdateQuizController(IMediator mediator)
        :base(mediator)
    {
        
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Update([FromRoute] Guid id, 
        [FromBody] AdminUpdateQuizRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AdminUpdateQuizCommand(id,request),cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successCode: StatusCodes.Status204NoContent);
    }
}

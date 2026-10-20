using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Quizzes.AdminManageQuestions.Requests;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Controllers;

[Route("api/admin/questions")]
public sealed class AdminManageQuestionsController : BaseController
{
    public AdminManageQuestionsController(IMediator mediator)
        :base(mediator)
    {
        
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> CreateQuestionWithOptions([FromBody] AdminAddQuestionWithOptionsRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminAddQuestionWithOptionsOrchestrator(request), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, "Question is added successfully with its options under the quiz.", StatusCodes.Status201Created);
    }

    [HttpPost("{questionId:Guid}/options")]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> CreateOptionOnQuestion([FromRoute] Guid questionId, [FromBody] AdminAddQuestionOptionRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminAddQuestionOptionOrchestrator(questionId, request), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, "Question option is added successfully under the question.", StatusCodes.Status201Created);
    }

    [HttpPut("{questionId:Guid}")]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> UpdateQuestion([FromRoute] Guid questionId, [FromBody] AdminUpdateQuestionRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminUpdateQuestionCommand(questionId, request), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successCode: StatusCodes.Status204NoContent);
    }

    [HttpPut("{questionId:Guid}/options/{optionId:Guid}")]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> UpdateQuestionOption([FromRoute] Guid questionId, [FromRoute] Guid optionId, [FromBody] string updatedOptionText, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminUpdateQuestionOptionOrchestrator(questionId, optionId, updatedOptionText), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successCode: StatusCodes.Status204NoContent);
    }

    [HttpPatch("{questionId:Guid}/options/{optionId:Guid}/correct-option")]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> SetCorrectQuestionOption([FromRoute] Guid questionId, [FromRoute] Guid optionId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminSetCorrectQuestionOptionOrchestrator(questionId, optionId), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successCode: StatusCodes.Status204NoContent);
    }

    [HttpDelete("{questionId:Guid}")]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> DeleteQuestion([FromRoute] Guid questionId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new AdminDeleteQuestionOrchestrator(questionId), cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "Question is deleted successfully !", successCode: StatusCodes.Status200OK);
    }
}

using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Quizzes.AdminManageQuestions.Requests;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Controllers;

[Route("api/admin/questions")]
public class AdminAddQuestionWithOptionsController : BaseController
{
    public AdminAddQuestionWithOptionsController(IMediator mediator)
        :base(mediator)
    {
        
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] AdminAddQuestionWithOptionsRequest request, CancellationToken cancellationToken =default)
    {
        var result = await _mediator.Send(new AdminAddQuestionWithOptionsOrchestrator(request));

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, "Question is added successfully with its options under the quiz.", StatusCodes.Status201Created);
    }
}

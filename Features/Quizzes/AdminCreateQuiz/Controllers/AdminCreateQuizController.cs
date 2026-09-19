using exam_system.Features.Quizzes.AdminCreateQuiz.Orchestrators;
using exam_system.Features.Quizzes.AdminCreateQuiz.Requests;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers;

[Route("api/admin/quizzes")]
[ApiController]
public class AdminCreateQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminCreateQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] AdminCreateQuizRequest request)
    {
        var result = await _mediator.Send(new AdminCreateQuizOrchestrator(request));

        if (result.IsFailure)
            return ApiResponse.Fail(result.Error.Message, StatusCodes.Status500InternalServerError, new Dictionary<string, string[]>() { [result.Error.Code] = [result.Error.Message] });

        return ApiResponse.Ok(message: "Quiz is created successfully !", statusCode: StatusCodes.Status201Created);
    }
}

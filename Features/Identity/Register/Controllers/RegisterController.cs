using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Orchestrators;
using exam_system.Features.Identity.Register.Requests;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Identity.Register.Controllers;

/// <summary>
/// Exposes the public registration endpoint under <c>/api/auth/register</c>.
/// </summary>
/// <param name="mediator">MediatR mediator used to dispatch the registration orchestrator.</param>
[Route("/api/auth/register")]
public class RegisterController(IMediator mediator) : BaseController(mediator)
{
    protected readonly new IMediator _mediator = mediator;

    /// <summary>
    /// Registers a new user account and sends an email-verification OTP.
    /// </summary>
    /// <param name="request">The registration payload containing full name, email and password.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>
    /// A <c>201 Created</c> <see cref="ApiResponse"/> when registration succeeds,
    /// or an error response derived from the failed <c>Result</c>.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] RegisterRequest request , CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RegisterOrchestrator(request) , cancellationToken);

        if (result.IsFailure)
            return FromResult(result);

        return FromResult(result, successMessage: "User Is created successfully", successCode: StatusCodes.Status201Created);
    }

}

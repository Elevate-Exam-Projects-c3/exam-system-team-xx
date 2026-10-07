using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Identity.Register.Orchestrators;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Identity.Register.Handlers;

/// <summary>
/// Orchestrates the registration flow by chaining the email-existence check and account creation.
/// </summary>
/// <param name="mediator">MediatR mediator used to dispatch the underlying query and command.</param>
public sealed class RegisterOrchestratorHandler(IMediator mediator) : IRequestHandler<RegisterOrchestrator, Result>
{
    /// <summary>
    /// Runs the registration workflow for the supplied request.
    /// </summary>
    /// <param name="request">The orchestrator request carrying the client registration data.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operations.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the user is created and the OTP is sent,
    /// otherwise a failure describing the problem.
    /// </returns>
    public async Task<Result> Handle(RegisterOrchestrator request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result.Failure(Error.NotFound("RegisterOrchestrator.NotFound", "request"));

        // checking if the mail already exists or not
        var result = await mediator.Send(new UserExistsQuery(request.RegisterRequest.Email));

        // if return failed if the email exists
        if (result.IsSuccessful)
            return Result.Failure(Error.Validation("RegisterOrchestrator.Validation", "Email already exists"));

        // save the user in the DB
        result = await mediator.Send(new RegisterCommand(request.RegisterRequest));

        return Result.Success();
    }
}
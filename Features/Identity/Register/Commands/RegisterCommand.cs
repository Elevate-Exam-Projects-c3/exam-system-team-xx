using exam_system.Features.Identity.Register.Requests;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Identity.Register.Commands;

/// <summary>
/// Command that creates a new user account from the supplied registration request.
/// </summary>
/// <param name="registerRequest">The registration data used to build and persist the user.</param>
public record RegisterCommand(RegisterRequest registerRequest) : IRequest<Result>;
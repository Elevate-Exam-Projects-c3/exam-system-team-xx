using exam_system.Features.Identity.Register.Requests;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Identity.Register.Orchestrators;

/// <summary>
/// Request that orchestrates the full registration flow: it first checks that the email is free,
/// then delegates account creation and OTP delivery to the underlying command handler.
/// </summary>
/// <param name="RegisterRequest">The registration data supplied by the client.</param>
public sealed record RegisterOrchestrator(RegisterRequest RegisterRequest) : IRequest<Result>;
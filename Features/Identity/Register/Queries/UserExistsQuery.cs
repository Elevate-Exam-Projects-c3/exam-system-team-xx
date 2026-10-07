using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Identity.Register.Queries;

/// <summary>
/// Query that checks whether a user with the given email already exists.
/// </summary>
/// <param name="email">The email address to look up.</param>
/// <remarks>
/// Returns a successful <see cref="Result"/> when the user exists and a
/// <c>NotFound</c> failure when no user matches the email.
/// </remarks>
public sealed record UserExistsQuery(string email) : IRequest<Result>;
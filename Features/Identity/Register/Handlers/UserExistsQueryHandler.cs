using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Features.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace exam_system.Features.Identity.Register.Handlers;

/// <summary>
/// Handles <see cref="UserExistsQuery"/> by looking the user up via ASP.NET Identity.
/// </summary>
/// <param name="userManager">Identity user manager used to query the user store.</param>
public sealed class UserExistsQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<UserExistsQuery, Result>
{
    /// <summary>
    /// Checks whether a user with the requested email exists.
    /// </summary>
    /// <param name="request">The query containing the email to look up.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the user exists, or a <c>NotFound</c> failure otherwise.
    /// </returns>
    public async Task<Result> Handle(UserExistsQuery request, CancellationToken cancellationToken)
    {
        // checking if the user exists by checking his/her mail in the DB
        if((await userManager.FindByEmailAsync(request.email)) is null)
        {
            return Result.Failure(Error.NotFound("user.notfound","user not found"));
        }
        return Result.Success();
    }
}

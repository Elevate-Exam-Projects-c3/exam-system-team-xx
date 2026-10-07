using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Features.Shared.Services;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace exam_system.Features.Identity.Register.Handlers;

/// <summary>
/// Handles <see cref="RegisterCommand"/> by creating the Identity user, assigning the default role
/// and triggering email-verification OTP delivery.
/// </summary>
/// <param name="userManager">Identity user manager used to create users and assign roles.</param>
/// <param name="roleManager">Identity role manager used to verify the default role exists.</param>
/// <param name="otpService">Service that generates and sends the email-verification OTP.</param>
/// <param name="mapper">Mapster mapper used to project the request and Identity errors.</param>
public sealed class RegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOtpService otpService,
    IMapper mapper) : IRequestHandler<RegisterCommand, Result>
{
    /// <summary>
    /// Creates a new user account and sends the verification OTP.
    /// </summary>
    /// <param name="request">The command containing the registration data.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operations.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the user is created and the OTP is sent, otherwise a failure.
    /// If adding the user to the role fails, the just-created user is rolled back (deleted).
    /// </returns>
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // mapping the userInfo that is passed from the request to the ApplicationUser type
        var user = mapper.Map<ApplicationUser>(request.registerRequest);

        user.UserName = request.registerRequest.Email;
        user.AccountStatus = AccountStatus.Pending;
        user.EmailConfirmed = false;

        // adding the user to the DB
        var identityResult = await userManager.CreateAsync(
            user,
            request.registerRequest.Password);

        // if the user isn't addded successfully then return an error
        if (!identityResult.Succeeded)
        {
            var errors = mapper.Map<IEnumerable<Error>>(identityResult.Errors);
            return Result.Failure(errors);
        }

        // check the exsistance of the role
        if (!(await roleManager.RoleExistsAsync(RoleNames.Student)))
        {
            return Result.Failure(Error.Validation("RegisterCommand.NotFound" , "Role doesn't exist"));
        }

        // Adding user to the Role
        var result = await userManager.AddToRoleAsync(user, RoleNames.Student);

        // Roll back the created user if the role assignment failed to avoid a half-created account.
        if (!result.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Result.Failure(mapper.Map<IEnumerable<Error>>(result.Errors));
        }
        
        // generating and sending the Otp
        var otpResult = await otpService.GenerateAndSendEmailVerificationOtpAsync(user, cancellationToken);

        return otpResult;
    }
}
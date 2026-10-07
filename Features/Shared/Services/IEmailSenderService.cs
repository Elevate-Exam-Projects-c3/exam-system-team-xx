using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared.ResultPattern;

namespace exam_system.Features.Shared.Services;

/// <summary>
/// Defines operations for sending transactional emails, such as OTP verification codes.
/// </summary>
public interface IEmailSenderService
{
    /// <summary>
    /// Sends an OTP verification code to the specified user by email.
    /// </summary>
    /// <param name="user">The recipient user (its email address is used as the destination).</param>
    /// <param name="otp">The OTP code to include in the message body.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the email is sent, otherwise a failure.
    /// </returns>
    Task<Result> SendOtpEmailAsync(ApplicationUser user, string otp, CancellationToken cancellationToken);
}

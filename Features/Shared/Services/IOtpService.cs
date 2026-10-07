using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared.ResultPattern;

namespace exam_system.Features.Shared.Services;

/// <summary>
/// Defines operations for generating and delivering OTP (one-time password) codes.
/// </summary>
public interface IOtpService
{
    /// <summary>
    /// Generates an email-verification OTP, persists it and sends it to the user's email address.
    /// </summary>
    /// <param name="user">The user for whom the verification OTP is generated.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the OTP is stored and the email is sent,
    /// otherwise a failure describing the problem.
    /// </returns>
    Task<Result> GenerateAndSendEmailVerificationOtpAsync(ApplicationUser user, CancellationToken cancellationToken);
}

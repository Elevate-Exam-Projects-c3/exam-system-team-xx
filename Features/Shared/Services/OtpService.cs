using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.Identity.Context;

namespace exam_system.Features.Shared.Services;

/// <summary>
/// Implementation of <see cref="IOtpService"/> that generates a six-digit OTP,
/// stores a BCrypt hash of it and sends the plain code by email.
/// </summary>
/// <param name="context">Database context used to persist generated OTP records.</param>
/// <param name="emailSender">Service used to deliver the OTP email.</param>
public sealed class OtpService(AuthDbContext context, IEmailSenderService emailSender) : IOtpService
{
    /// <summary>
    /// Creates a new email-verification OTP for the user, saves it and sends it by email.
    /// </summary>
    /// <param name="user">The user for whom the OTP is generated.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operations.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the OTP is persisted and emailed,
    /// otherwise a failure describing the problem.
    /// </returns>
    public async Task<Result> GenerateAndSendEmailVerificationOtpAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        // The OTP cannot be generated without a destination email address.
        if (string.IsNullOrEmpty(user.Email))
        {
            return Result.Failure(new Error("OtpGeneration.EmailNotFound", "Email not found", ErrorType.Validation));
        }

        // Generate a random six-digit code (zero-padded, e.g. "004271").
        var code = Random.Shared.Next(0, 1_000_000).ToString("D6");
        var Otp = new EmailVerificationOtp
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Email = user.Email,
            OtpHash = BCrypt.Net.BCrypt.HashPassword(code, workFactor: 12), // this workFactor because of the salt >= 12
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            AttemptCount = 0,
            IsUsed = false
        };
        context.EmailVerificationOtp.Add(Otp);

        await context.SaveChangesAsync(cancellationToken);

        try
        {
            await emailSender.SendOtpEmailAsync(user, code, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("OtpGeneration.EmailError", "Error happened while sending the otp", ErrorType.Validation));
        }
        return Result.Success();
    }
}

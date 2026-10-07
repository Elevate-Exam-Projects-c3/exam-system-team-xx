using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared.ResultPattern;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace exam_system.Features.Shared.Services;

/// <summary>
/// Implementation of <see cref="IEmailSenderService"/> that sends mail through Gmail's SMTP server using MailKit.
/// </summary>
/// <param name="configuration">Application configuration providing the <c>Email:Username</c> and <c>Email:Password</c> credentials.</param>
public class EmailSenderService(IConfiguration configuration) : IEmailSenderService
{
    /// <summary>
    /// Connects to Gmail over STARTTLS, authenticates and sends the OTP verification email.
    /// </summary>
    /// <param name="user">The recipient user (its email address is used as the destination).</param>
    /// <param name="otp">The OTP code embedded in the message body.</param>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A successful <see cref="Result"/> when the message is sent, otherwise a failure.
    /// </returns>
    public async Task<Result> SendOtpEmailAsync(ApplicationUser user, string otp, CancellationToken cancellationToken)
    {
        try
        {
            var message = new MimeMessage();

            // adding the from part
            message.From.Add(new MailboxAddress("My App", configuration["Email:Username"]!));
            // adding the to part
            message.To.Add(MailboxAddress.Parse(user.Email!));
            // adding the message
            // 1. Subject
            message.Subject = "Your verification code";
            // 2. body
            message.Body = new TextPart("Plain")
            {
                Text = $"Your OTP is: {otp}. It expires in 10 minutes."
            };

            using var client = new SmtpClient();

            // connecting to smtp.google.com
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            // authenticating the sender
            var username = configuration["Email:Username"];
            var password = configuration["Email:Password"];
            await client.AuthenticateAsync(username!, password!);

            // sending the message
            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
        catch
        {
            return Result.Failure(new Error("EmailSenderService.NotSent" , "message not sent" , ErrorType.Validation));
        }

        return Result.Success();
    }
}

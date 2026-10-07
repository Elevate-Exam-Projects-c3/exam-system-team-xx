namespace exam_system.Features.Identity.Register.Requests;

/// <summary>
/// Represents the payload sent by a client to register a new account.
/// </summary>
/// <param name="FullName">The full display name of the new user.</param>
/// <param name="Email">The email address that will be used as the user name and login identifier.</param>
/// <param name="Password">The plain-text password for the new account.</param>
public sealed record RegisterRequest(string FullName, string Email, string Password);

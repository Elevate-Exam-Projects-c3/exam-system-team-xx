using BCrypt.Net;
using exam_system.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace exam_system.Features.Identity.Register.HashingPass;

/// <summary>
/// Password hasher implementation for <see cref="ApplicationUser"/> backed by BCrypt.
/// </summary>
/// <remarks>
/// BCrypt automatically generates a per-password salt and embeds it in the resulting hash,
/// so no separate salt storage is required.
/// </remarks>
public class BCryptPasswordHasher : IPasswordHasher<ApplicationUser>
{
    /// <summary>
    /// BCrypt work factor (cost). 12 keeps hashing reasonably slow to resist brute-force attacks.
    /// </summary>
    private const int WorkFactor = 12;

    /// <summary>
    /// Hashes the supplied plain-text password using BCrypt.
    /// </summary>
    /// <param name="user">The user whose password is being hashed. Not used by BCrypt.</param>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>The BCrypt hash (salt included) of the password.</returns>
    public string HashPassword(ApplicationUser user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    /// <summary>
    /// Verifies a plain-text password against a stored BCrypt hash.
    /// </summary>
    /// <param name="user">The user whose password is being verified. Not used by BCrypt.</param>
    /// <param name="hashedPassword">The previously stored BCrypt hash.</param>
    /// <param name="providedPassword">The plain-text password supplied for verification.</param>
    /// <returns>
    /// <see cref="PasswordVerificationResult.Success"/> when the password matches,
    /// otherwise <see cref="PasswordVerificationResult.Failed"/>.
    /// </returns>
    public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
    {
        bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        return isValid ? PasswordVerificationResult.Success: PasswordVerificationResult.Failed;
    }
}

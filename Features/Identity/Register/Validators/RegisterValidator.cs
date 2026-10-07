
using exam_system.Features.Identity.Register.Commands;
using FluentValidation;

namespace exam_system.Features.Identity.Register.Validators;

/// <summary>
/// FluentValidation validator that defines the input rules for a <see cref="RegisterCommand"/>.
/// </summary>
/// <remarks>
/// The validator is executed by the MediatR validation pipeline before the
/// <c>RegisterCommandHandler</c> runs, so the handler can assume the request is well formed.
/// </remarks>
public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Configures the validation rules for the registration request:
    /// a non-empty full name (2-100 characters), a valid email address and a non-empty password.
    /// </summary>
    public RegisterValidator()
    {
        // Full name is required and must be between 2 and 100 characters.
        RuleFor(u => u.registerRequest.FullName)
            .NotEmpty().Length(2,100);

        // Email is required and must be a syntactically valid address.
        RuleFor(u => u.registerRequest.Email)
            .NotEmpty().EmailAddress();

        // Password is required. Additional strength rules can be added here.
        RuleFor(u => u.registerRequest.Password)
            .NotEmpty();
    }
}

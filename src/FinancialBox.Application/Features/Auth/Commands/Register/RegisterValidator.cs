using FinancialBox.Domain.Features.Users.ValueObjects;
using FluentValidation;

namespace FinancialBox.Application.Features.Auth.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithErrorCode("FirstName.Empty").WithMessage("First name is required.")
            .MinimumLength(2).WithErrorCode("FirstName.TooShort").WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(100).WithErrorCode("FirstName.TooLong").WithMessage("First name must be at most 100 characters long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithErrorCode("LastName.Empty").WithMessage("Last name is required.")
            .MinimumLength(2).WithErrorCode("LastName.TooShort").WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(100).WithErrorCode("LastName.TooLong").WithMessage("Last name must be at most 100 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("Email.Empty").WithMessage("Email is required.")
            .Matches(Email.EmailRegex).WithErrorCode("Email.InvalidFormat").WithMessage("Invalid email format.")
            .MaximumLength(255).WithErrorCode("Email.TooLong").WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("Password.Empty").WithMessage("Password is required.")
            .MinimumLength(8).WithErrorCode("Password.TooShort").WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100).WithErrorCode("Password.TooLong").WithMessage("Password must be at most 100 characters long.")
            .Matches("[A-Z]").WithErrorCode("Password.MissingUppercase").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithErrorCode("Password.MissingLowercase").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithErrorCode("Password.MissingDigit").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithErrorCode("Password.MissingSpecialChar").WithMessage("Password must contain at least one special character.");
    }
}

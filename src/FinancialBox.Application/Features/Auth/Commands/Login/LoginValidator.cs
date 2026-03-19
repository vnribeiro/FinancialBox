using FluentValidation;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("Email.Empty").WithMessage("Email is required.")
            .MaximumLength(255).WithErrorCode("Email.TooLong").WithMessage("Email must be at most 255 characters long.")
            .EmailAddress().WithErrorCode("Email.Invalid").WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("Password.Empty").WithMessage("Password is required.")
            .MinimumLength(8).WithErrorCode("Password.TooShort").WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100).WithErrorCode("Password.TooLong").WithMessage("Password must be at most 100 characters long.");
    }
}

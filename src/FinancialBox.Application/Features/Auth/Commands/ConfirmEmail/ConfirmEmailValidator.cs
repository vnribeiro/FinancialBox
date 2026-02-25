using FinancialBox.Domain.Features.Users.ValueObjects;
using FluentValidation;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("Email.Empty").WithMessage("Email is required.")
            .Matches(Email.EmailRegex).WithErrorCode("Email.InvalidFormat").WithMessage("Email is invalid.")
            .MaximumLength(255).WithErrorCode("Email.TooLong").WithMessage("Email must be at most 255 characters long.");

        RuleFor(x => x.Code)
            .NotEmpty().WithErrorCode("Code.Empty").WithMessage("Code is required.")
            .Matches(@"^\d{6}$").WithErrorCode("Code.InvalidFormat").WithMessage("Code must be exactly 6 digits.");
    }
}

using FinancialBox.Domain.Features.Users.ValueObjects;
using FluentValidation;

namespace FinancialBox.Application.Features.Auth.Commands.ResendConfirmation;

public class ResendConfirmationValidator : AbstractValidator<ResendConfirmationCommand>
{
    public ResendConfirmationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("Email.Empty").WithMessage("Email is required.")
            .Matches(Email.EmailRegex).WithErrorCode("Email.InvalidFormat").WithMessage("Email is invalid.")
            .MaximumLength(255).WithErrorCode("Email.TooLong").WithMessage("Email must be at most 255 characters long.");
    }
}

using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FluentValidation;

namespace FinancialBox.Application.Features.Auth.Commands.ResendConfirmation;

public class ResendConfirmationValidator : AbstractValidator<ResendConfirmationCommand>
{
    public ResendConfirmationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("Email.Empty").WithMessage("Email is required.")
            .MaximumLength(255).WithErrorCode("Email.TooLong").WithMessage("Email must be at most 255 characters long.")
            .EmailAddress().WithErrorCode("Email.Invalid").WithMessage("Email must be a valid email address.");
    }
}

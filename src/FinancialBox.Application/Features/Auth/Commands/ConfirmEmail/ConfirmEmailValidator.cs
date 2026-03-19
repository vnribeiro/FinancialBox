using FluentValidation;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithErrorCode("Token.Empty").WithMessage("Token is required.")
            .Must(t => Guid.TryParse(t, out _)).WithErrorCode("Token.InvalidFormat").WithMessage("Token must be a valid GUID.");
    }
}

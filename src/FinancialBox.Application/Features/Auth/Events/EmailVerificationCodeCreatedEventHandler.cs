using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Application.DomainEvents;
using FinancialBox.Domain.Features.Users.Events;

namespace FinancialBox.Application.Features.Auth.Events;

public sealed class EmailVerificationCodeCreatedEventHandler(IEmailService emailService)
    : IDomainEventHandler<EmailVerificationCodeCreatedEvent>
{
    public Task Handle(EmailVerificationCodeCreatedEvent notification, CancellationToken cancellationToken)
        => emailService.SendVerificationCodeAsync(notification.Email, notification.PlainCode, cancellationToken);
}

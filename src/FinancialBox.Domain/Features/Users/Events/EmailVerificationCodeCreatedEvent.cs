using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Domain.Features.Users.Events;

public record EmailVerificationCodeCreatedEvent(
    Guid UserId,
    string Email,
    string PlainCode) : BaseDomainEvent;

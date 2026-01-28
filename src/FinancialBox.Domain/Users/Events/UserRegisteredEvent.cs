using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Domain.Users.Events;

public record UserRegisteredEvent(Guid UserId, string Email) : BaseDomainEvent;

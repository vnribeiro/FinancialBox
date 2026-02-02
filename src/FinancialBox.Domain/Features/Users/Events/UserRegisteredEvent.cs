using FinancialBox.Domain.DomainEvents;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Domain.Features.Users.Events;

public record UserRegisteredEvent(Guid UserId, Email Email) : BaseDomainEvent;

using FinancialBox.Domain.DomainEvents;
using FinancialBox.Domain.Features.User.ValueObjects;

namespace FinancialBox.Domain.Features.User.Events;

public record UserRegisteredEvent(Guid UserId, Email Email) : BaseDomainEvent;

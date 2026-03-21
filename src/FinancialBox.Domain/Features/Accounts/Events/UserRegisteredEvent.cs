using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Domain.Features.Accounts.Events;

public record UserRegisteredEvent(
    Guid AccountId, 
    string Email, 
    string Token) : BaseDomainEvent;
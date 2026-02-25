using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Domain.Common;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}



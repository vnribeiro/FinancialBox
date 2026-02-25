using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Domain.Common;

public abstract class AggregateRoot : BaseEntity, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot() {}

    protected AggregateRoot(Guid id) : base(id) {}

    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}

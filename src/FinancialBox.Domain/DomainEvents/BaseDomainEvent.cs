namespace FinancialBox.Domain.DomainEvents;

public abstract record BaseDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

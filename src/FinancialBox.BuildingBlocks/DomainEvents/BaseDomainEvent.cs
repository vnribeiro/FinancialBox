namespace FinancialBox.BuildingBlocks.DomainEvents;

public abstract record BaseDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
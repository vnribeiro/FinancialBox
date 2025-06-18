namespace FinancialBox.BuildingBlocks.DomainEvents;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent notification, CancellationToken cancellationToken);
}
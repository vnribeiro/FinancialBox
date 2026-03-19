using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Application.DomainEvents;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent notification, CancellationToken cancellationToken);
}

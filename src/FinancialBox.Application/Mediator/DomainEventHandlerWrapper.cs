using FinancialBox.Application.DomainEvents;
using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Application.Mediator;

internal sealed class DomainEventHandlerWrapper<TEvent>(IDomainEventHandler<TEvent> domainEventHandler) : IDomainEventHandlerWrapper
    where TEvent : IDomainEvent
{
    private readonly IDomainEventHandler<TEvent> _domainEventHandler = domainEventHandler;

    public Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        => _domainEventHandler.Handle((TEvent)notification, cancellationToken);
}

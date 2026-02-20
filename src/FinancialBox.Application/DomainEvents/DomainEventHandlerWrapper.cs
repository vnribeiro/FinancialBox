using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Application.DomainEvents;

internal sealed class DomainEventHandlerWrapper<TEvent> : IDomainEventHandlerWrapper
    where TEvent : IDomainEvent
{
    private readonly IDomainEventHandler<TEvent> _inner;

    public DomainEventHandlerWrapper(IDomainEventHandler<TEvent> inner)
        => _inner = inner;

    public Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        => _inner.Handle((TEvent)notification, cancellationToken);
}

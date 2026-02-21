using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Application.Mediator;

internal interface IDomainEventHandlerWrapper
{
    Task Handle(IDomainEvent notification, CancellationToken cancellationToken);
}

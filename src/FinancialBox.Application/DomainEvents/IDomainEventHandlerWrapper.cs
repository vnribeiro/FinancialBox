using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Application.DomainEvents;

internal interface IDomainEventHandlerWrapper
{
    Task Handle(IDomainEvent notification, CancellationToken cancellationToken);
}

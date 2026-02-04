using FinancialBox.Domain.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts;

namespace FinancialBox.Infrastructure.Persistence;

internal class UnitOfWork(AppDbContext context, IMediator mediator) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var hasChanges = await context.SaveChangesAsync(cancellationToken) > 0;

        if (hasChanges)
        {
            await PublishDomainEventsAsync(cancellationToken);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.PublishAsync(domainEvent, cancellationToken);
        }

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());
    }
}

using FinancialBox.Domain.Common;
using FinancialBox.Domain.DomainEvents;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts;

namespace FinancialBox.Infrastructure.Persistence;

internal class UnitOfWork(AppDbContext context, IMediator mediator) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var aggregates = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(e => e.DomainEvents)
            .ToList();

        aggregates.ForEach(e => e.ClearDomainEvents());

        await context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await mediator.PublishAsync(domainEvent, cancellationToken);
    }
}

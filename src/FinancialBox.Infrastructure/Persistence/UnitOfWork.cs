using System.Text.Json;
using FinancialBox.Application.Abstractions;
using FinancialBox.Domain.Common;
using FinancialBox.Infrastructure.Persistence.Outbox;

namespace FinancialBox.Infrastructure.Persistence;

internal sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var aggregates = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var outboxMessages = aggregates
            .SelectMany(e => e.DomainEvents)
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                CreatedAtUtc = DateTime.UtcNow
            })
            .ToList();

        aggregates.ForEach(e => e.ClearDomainEvents());

        if (outboxMessages.Count > 0)
            await context.OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}

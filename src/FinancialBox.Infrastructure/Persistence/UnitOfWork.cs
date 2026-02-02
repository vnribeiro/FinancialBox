using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Domain.Common;
using FinancialBox.Application.Contracts.Messaging;

namespace FinancialBox.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context, IMediator mediator) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private readonly IMediator _mediator = mediator;

    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        if (!success)
        {
            return success;
        }

        await PublishDomainEventsAsync(cancellationToken);

        return success;
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.PublishAsync(domainEvent, cancellationToken);
        }

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());
    }
}

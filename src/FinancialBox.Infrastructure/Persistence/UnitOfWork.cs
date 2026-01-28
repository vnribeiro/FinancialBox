using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.Application.Persistence;

namespace FinancialBox.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;

    public UnitOfWork(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        //var success = await _context.SaveChangesAsync(cancellationToken) > 0;
        var success = true;

        if (!success)
            return success;

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
            await _mediator.PublishAsync(domainEvent, cancellationToken);

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());
    }
}

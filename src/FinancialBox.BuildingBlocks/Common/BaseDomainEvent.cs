using FinancialBox.BuildingBlocks.Mediator;

namespace FinancialBox.BuildingBlocks.Common;

public abstract record BaseDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
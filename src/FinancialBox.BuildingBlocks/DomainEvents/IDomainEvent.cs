namespace FinancialBox.BuildingBlocks.DomainEvents;

public interface IDomainEvent
{
     DateTime OccurredOn { get; init; }
};
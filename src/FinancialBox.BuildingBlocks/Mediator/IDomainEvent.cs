namespace FinancialBox.BuildingBlocks.Mediator;

public interface IDomainEvent
{
     DateTime OccurredOn { get; init; }
};
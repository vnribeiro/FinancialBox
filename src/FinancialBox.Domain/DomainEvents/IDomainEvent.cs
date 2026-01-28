namespace FinancialBox.Domain.DomainEvents;

public interface IDomainEvent
{
     DateTime OccurredOn { get; init; }
};

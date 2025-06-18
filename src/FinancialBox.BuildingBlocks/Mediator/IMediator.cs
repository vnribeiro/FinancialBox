using FinancialBox.BuildingBlocks.DomainEvents;

namespace FinancialBox.BuildingBlocks.Mediator;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    Task PublishAsync<TEvent>(TEvent notification, CancellationToken cancellationToken = default) where TEvent : IDomainEvent;
}
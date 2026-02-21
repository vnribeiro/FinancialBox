using FinancialBox.Application.Common;
using FinancialBox.Domain.DomainEvents;

namespace FinancialBox.Application.Abstractions.Pipeline;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : IResult<TResponse>;

    Task PublishAsync<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}

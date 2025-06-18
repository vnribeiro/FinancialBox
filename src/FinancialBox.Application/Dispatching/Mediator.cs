using FinancialBox.BuildingBlocks.DomainEvents;
using FinancialBox.BuildingBlocks.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application.Dispatching;

public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = _provider.GetService(handlerType);

        if (handler is null)
            throw new InvalidOperationException($"Handler not registered for '{requestType.Name}'. Check your DI configuration.");

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _provider.GetServices(behaviorType).Cast<object>().ToList();

        Func<Task<TResponse>> pipeline = () => ((dynamic)handler).Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = () => ((dynamic)behavior).Handle((dynamic)request, next, cancellationToken);
        }

        return await pipeline();
    }

    public async Task PublishAsync<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(notification.GetType());
        var handlers = _provider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            if (handler == null)
                throw new InvalidOperationException($"Handler instance is null for '{notification.GetType().Name}'. Check your DI configuration.");

            await ((dynamic)handler).Handle((dynamic)notification, cancellationToken);
        }
    }
}
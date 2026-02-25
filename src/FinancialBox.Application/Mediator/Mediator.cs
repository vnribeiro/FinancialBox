using FinancialBox.Domain.DomainEvents;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinancialBox.Application.Mediator;

public class Mediator(IServiceProvider provider, ILogger<Mediator> logger) : IMediator
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = provider.GetService(handlerType) ?? throw new InvalidOperationException($"Handler not registered for '{requestType.Name}'. Check your DI configuration.");

        var handlerWrapperType = typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse));
        var handlerWrapper = (IRequestHandlerWrapper<TResponse>)Activator.CreateInstance(handlerWrapperType, handler)!;

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = provider.GetServices(behaviorType).ToList();

        RequestHandlerDelegate<TResponse> pipeline = () => handlerWrapper.Handle(request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = pipeline;
            var behaviorWrapperType = typeof(PipelineBehaviorWrapper<,>).MakeGenericType(requestType, typeof(TResponse));
            var behaviorWrapper = (IPipelineBehaviorWrapper<TResponse>)Activator.CreateInstance(behaviorWrapperType, behavior)!;
            pipeline = () => behaviorWrapper.Handle(request, next, cancellationToken);
        }

        return await pipeline();
    }

    public async Task PublishAsync<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        var eventType = notification.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        var handlers = provider.GetServices(handlerType);
        var wrapperType = typeof(DomainEventHandlerWrapper<>).MakeGenericType(eventType);

        foreach (var handler in handlers)
        {
            try
            {
                var wrapper = (IDomainEventHandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
                await wrapper.Handle(notification, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Handler {HandlerName} failed for event {EventName}", handler!.GetType().Name, eventType.Name);
            }
        }
    }
}

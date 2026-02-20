using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.DomainEvents;
using FinancialBox.Domain.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application;

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
        var handler = _provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler not registered for '{requestType.Name}'. Check your DI configuration.");

        var handlerWrapperType = typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse));
        var handlerWrapper = (IRequestHandlerWrapper<TResponse>)Activator.CreateInstance(handlerWrapperType, handler)!;

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _provider.GetServices(behaviorType).ToList();

        Func<Task<TResponse>> pipeline = () => handlerWrapper.Handle(request, cancellationToken);

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
        var handlers = _provider.GetServices(handlerType);

        var wrapperType = typeof(DomainEventHandlerWrapper<>).MakeGenericType(eventType);

        foreach (var handler in handlers)
        {
            var wrapper = (IDomainEventHandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
            await wrapper.Handle(notification, cancellationToken);
        }
    }
}

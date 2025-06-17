using FinancialBox.BuildingBlocks.Behaviors;
using FinancialBox.BuildingBlocks.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application.Core.Mediator;

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
}
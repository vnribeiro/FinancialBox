using FinancialBox.BuildingBlocks.Behaviors;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application.Interceptors.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<T>> Send<T>(IRequest<Result<T>> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(T));
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(T));

        dynamic handler = _provider.GetRequiredService(handlerType);
        var behaviors = _provider.GetServices(behaviorType).Cast<dynamic>().ToList();

        Func<Task<Result<T>>> pipeline = () => handler.Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = () => behavior.Handle((dynamic)request, cancellationToken, next);
        }

        return await pipeline();
    }
}

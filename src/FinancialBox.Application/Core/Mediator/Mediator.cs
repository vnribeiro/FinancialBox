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

    public async Task<Result<TResponse>> SendAsync<TResponse>(
        IRequest<Result<TResponse>> request,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _provider.GetService(handlerType);

        if (handler is null)
        {
            var error = Error.InternalServerError(
                $"Handler resolution failed: No handler registered for '{requestType.Name}'. " +
                $"This likely indicates a service registration or DI configuration issue."
            );

            return Result<TResponse>.Failure(error);
        }

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _provider.GetServices(behaviorType).Cast<object>().ToList();

        Func<Task<Result<TResponse>>> pipeline = () => ((dynamic)handler).Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = () => ((dynamic)behavior).Handle((dynamic)request, next, cancellationToken);
        }

        return await pipeline();
    }
}


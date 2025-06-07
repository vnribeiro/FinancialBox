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

    public async Task<Result<TResponse>> Send<TResponse>(IRequest<Result<TResponse>> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = request is ICommand<TResponse>
            ? typeof(ICommandHandler<,>).MakeGenericType(requestType, typeof(TResponse))
            : typeof(IQueryHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));

        dynamic handler = _provider.GetRequiredService(handlerType);
        var behaviors = _provider.GetServices(behaviorType).Cast<dynamic>().ToList();

        Func<Task<Result<TResponse>>> pipeline = () => handler.Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = () => behavior.Handle((dynamic)request, cancellationToken, next);
        }

        return await pipeline();
    }
}

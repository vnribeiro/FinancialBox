using FinancialBox.Shared.Contracts.Behaviors;
using FinancialBox.Shared.Contracts.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application.Common.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public Mediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            dynamic handler = _provider.GetRequiredService(handlerType);

            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
            var behaviors = _provider.GetServices(behaviorType).Cast<object>().ToList();

            Func<CancellationToken, Task<TResponse>> pipeline = ct => handler.Handle((dynamic)request, ct);

            foreach (var behaviorObj in behaviors.AsEnumerable().Reverse())
            {
                dynamic behavior = behaviorObj;

                var next = pipeline;

                pipeline = ct => behavior.Handle((dynamic)request, next, ct);
            }

            return await pipeline(cancellationToken);
        }
    }
}

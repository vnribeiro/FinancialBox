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

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            dynamic handler = _provider.GetRequiredService(handlerType);

            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
            var behaviors = _provider.GetServices(behaviorType).Cast<dynamic>().ToList();

            Func<Task<TResponse>> pipeline = () => handler.Handle((dynamic)request);

            foreach (var behavior in behaviors.AsEnumerable().Reverse())
            {
                var next = pipeline;
                pipeline = () => behavior.Handle((dynamic)request, next);
            }

            return await pipeline();
        }
    }

}

using FinancialBox.Shared.Contracts.Behaviors;
using Microsoft.Extensions.Logging;

namespace FinancialBox.Application.Common.Behaviors
{
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

        public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            Func<CancellationToken, Task<TResponse>> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            try
            {
                _logger.LogInformation("Handling request: {RequestName}", requestName);

                var response = await next(cancellationToken);

                _logger.LogInformation("Successfully handled request: {RequestName}", requestName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling request: {RequestName}", requestName);
                throw;
            }
        }
    }
}
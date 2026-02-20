using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using Microsoft.Extensions.Logging;

namespace FinancialBox.Application.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            _logger.LogDebug("Handling request: {RequestName}", requestName);

            var response = await next();

            _logger.LogDebug("Successfully handled request: {RequestName}", requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling request: {RequestName}", requestName);

            var error = Error.UnexpectedServerError();

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error)!;
            }

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericType = typeof(TResponse).GetGenericArguments()[0];
                var method = typeof(Result<>)
                    .MakeGenericType(genericType)
                    .GetMethod(nameof(Result<object>.Failure), [typeof(Error)]);

                var result = method!.Invoke(null, [error]);
                return (TResponse)result!;
            }

            throw;
        }
    }
}

using FinancialBox.Application.Common;
using FinancialBox.Application.Abstractions.Pipeline;
using Microsoft.Extensions.Logging;

namespace FinancialBox.Application.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
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

            return TResponse.Failure(Error.UnexpectedServerError());
        }
    }
}

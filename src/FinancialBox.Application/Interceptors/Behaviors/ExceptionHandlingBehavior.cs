using FinancialBox.BuildingBlocks.Behaviors;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;
using Microsoft.Extensions.Logging;

namespace FinancialBox.Application.Interceptors.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<Result<TResponse>> 
    where TResponse : notnull
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<Result<TResponse>> Handle(
        TRequest request,
        Func<Task<Result<TResponse>>> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            _logger.LogInformation("Handling request: {RequestName}", requestName);

            var response = await next();

            _logger.LogInformation("Successfully handled request: {RequestName}", requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling request: {RequestName}", requestName);
            return Result<TResponse>.Failure(Error.InternalServerError());
        }
    }
}

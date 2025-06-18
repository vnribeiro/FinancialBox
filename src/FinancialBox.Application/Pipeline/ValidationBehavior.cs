using FinancialBox.BuildingBlocks.Common;
using FinancialBox.BuildingBlocks.Mediator;
using FluentValidation;

namespace FinancialBox.Application.Pipeline;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => !string.IsNullOrWhiteSpace(f.ErrorMessage))
            .ToList();

        if (!failures.Any())
            return await next();

        var messages = failures.Select(f => f.ErrorMessage).ToArray();

        if (typeof(TResponse) == typeof(Result))
        {
            var result = Result.Failure(messages);
            return (TResponse)(object)result!;
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericType = typeof(TResponse).GetGenericArguments()[0];
            var method = typeof(Result<>)
                .MakeGenericType(genericType)
                .GetMethod(nameof(Result<object>.Failure), [typeof(string[])]);

            var result = method!.Invoke(null, [messages]);
            return (TResponse)result!;
        }

        return await next();
    }
}

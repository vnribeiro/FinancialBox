using System.Collections.Concurrent;
using System.Reflection;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Domain.Primitives;
using FluentValidation;

namespace FinancialBox.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> _failureMethodCache = new();

    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var failureMethod = _failureMethodCache.GetOrAdd(typeof(TResponse), t =>
            t.GetMethod(nameof(Result.Failure), BindingFlags.Public | BindingFlags.Static, [typeof(IReadOnlyList<Error>)])
            ?? throw new InvalidOperationException($"{typeof(TRequest).Name} handler must return Result<T> to use ValidationBehavior."));

        var errors = failures
            .Select(f => Error.Validation(f.ErrorCode, f.ErrorMessage))
            .ToList();

        return (TResponse)failureMethod.Invoke(null, [errors])!;
    }
}

using FinancialBox.BuildingBlocks.Behaviors;
using FinancialBox.BuildingBlocks.Mediator;
using FinancialBox.BuildingBlocks.Result;
using FluentValidation;

namespace FinancialBox.Application.Interceptors.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<Result<TResponse>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<TResponse>> Handle(
        TRequest request,
        Func<Task<Result<TResponse>>> next,
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

        var messages = failures.Select(f => f.ErrorMessage).ToList();
        return Result<TResponse>.Failure(messages);
    }
}


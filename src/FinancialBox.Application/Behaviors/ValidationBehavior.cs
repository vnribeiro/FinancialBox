using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FluentValidation;

namespace FinancialBox.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

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
            .Where(f => !string.IsNullOrWhiteSpace(f.ErrorMessage))
            .ToList();

        if (!failures.Any())
            return await next();

        var messages = failures.Select(f => f.ErrorMessage).ToArray();

        return TResponse.Failure(Error.ValidationFailure(messages));
    }
}

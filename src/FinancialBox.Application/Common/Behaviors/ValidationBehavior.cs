using FinancialBox.Shared.Contracts.Behaviors;
using FinancialBox.Shared.Contracts.Mediator;
using FinancialBox.Shared.ResultObjects;
using FluentValidation;
using FluentValidation.Results;

namespace FinancialBox.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            Func<CancellationToken, Task<TResponse>> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.SelectMany(r => r.Errors).ToList();

            if (failures.Any())
            {
                var messages = failures
                    .Where(f => !string.IsNullOrWhiteSpace(f.ErrorMessage))
                    .Select(f => f.ErrorMessage)
                    .ToArray();

                var resultType = typeof(TResponse);
                var valueType = resultType.GenericTypeArguments.FirstOrDefault();

                if (valueType is null)
                    throw new InvalidOperationException("Unable to create validation result.");

                var failureMethod = typeof(Result<>)
                    .MakeGenericType(valueType)
                    .GetMethod(nameof(Result<object>.Failure), [typeof(IEnumerable<string>)]);

                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, [messages])!;
                }
            }

            return await next(cancellationToken);
        }
    }
}

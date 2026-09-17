using FluentValidation;
using MediatR;

namespace exam_system.Shared.Behavior
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Validation the request and return the result if validation fails, otherwise call the next handler in the pipeline.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                return await next();
            }

            // If not then we have any validators, we will validate the request and return the result if validation fails.
            Error[] errors = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .Select(failure => new Error(
                        failure.PropertyName,
                        failure.ErrorMessage))
                .Distinct()
                .ToArray();

            if (errors.Any())
            {
                // Return Validation Result
                return CreateValidationResult<TResponse>(errors);

            }
            return await next();

        }

        /// <summary>
        /// Static method to create a validation result of type TResult with the given errors. 
        /// This method uses reflection to create an instance of ValidationResult<TResult> 
        /// if TResult is a generic type, or ValidationResult if TResult is not generic.
        /// This is useful for generic Result types, where we want to 
        /// return a ValidationResult with the same generic type as TResult.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static TResult CreateValidationResult<TResult>(Error[] errors) where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (ValidationResult.WithErrors(errors) as TResult)!;
            }

            object? validationResult = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                // Why the first one
                .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
                .GetMethod(nameof(ValidationResult.WithErrors))!
                .Invoke(null, new object?[] { errors });

            return (TResult)validationResult!;
        }
    }
}

using exam_system.Features.Shared.ResultPattern;
using FluentValidation;
using MediatR;

namespace exam_system.Features.Shared.Behaviors;

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
        if (!_validators.Any())
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
                    failure.ErrorMessage,
                    ErrorType.Validation))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            // Return Validation Result
            return (TResponse) Result.Failure(errors);
        }

        return await next();
    }
}

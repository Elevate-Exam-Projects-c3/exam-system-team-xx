namespace exam_system.Features.Shared.ResultPattern;

public class Result
{
    /* Properties */
    public bool IsSuccessful { get; }
    public bool IsFailure => !IsSuccessful;
    public IReadOnlyList<Error> Errors { get; }

    /* Constructors */
    protected Result(bool isSuccessful, Error error)
    {
        if (isSuccessful && error != Error.None)
            throw new ArgumentException("Successful result cannot have an error !");

        if (!isSuccessful && error == Error.None)
            throw new ArgumentException("Failure result must have an error !");

        IsSuccessful = isSuccessful;
        Errors = isSuccessful ? [] : [error];
    }

    protected Result(IEnumerable<Error> errors)
    {
        var errorsList = errors?.ToList() ?? throw new ArgumentNullException(nameof(errors), "Failure result must have non null errors collection !");

        if (!errorsList.Any() || errorsList.Any(e => e == Error.None))
            throw new ArgumentException("Failure result must have at least one non-None error!");

        IsSuccessful = false;
        Errors = errorsList;
    }

    /* Mehtods */
    public static Result Success()
        => new(true, Error.None);
    public static Result Failure(Error error)
        => new(false, error);
    public static Result Failure(IEnumerable<Error> errors)
        => new(errors);

    /* Operators */
    public static implicit operator Result(Error error)
        => Failure(error);
}
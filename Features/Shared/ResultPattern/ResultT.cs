namespace exam_system.Features.Shared.ResultPattern;
public sealed class Result<TValue> : Result
{
    /* Properties */
    private readonly TValue? _value;
    public TValue Value
        => IsSuccessful ?
        _value! : throw new InvalidOperationException("Cannot access a value for a failure result");

    /* Constructor Overloads */
    private Result(TValue value)
        : base(true, Error.None)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value), "Value shall not be nullable in case of successful result !");
        _value = value;
    }
    private Result(Error error)
        : base(false, error)
    {
        _value = default;
    }
    private Result(IEnumerable<Error> errors)
        : base(errors)
    {
        _value = default;
    }

    /* Mehtods */
    public static Result<TValue> Success(TValue value)
        => new(value);
    public new static Result<TValue> Failure(Error error)
        => new(error);
    public new static Result<TValue> Failure(IEnumerable<Error> errors)
        => new(errors);
    public TResult Match<TResult>(Func<TValue, TResult> onSuccess,
        Func<IEnumerable<Error>, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);
        return IsSuccessful ? onSuccess(Value) : onFailure(Errors);
    }

    /* Operators */
    public static implicit operator Result<TValue>(TValue value)
        => Success(value);
    public static implicit operator Result<TValue>(Error error)
        => Failure(error);
}
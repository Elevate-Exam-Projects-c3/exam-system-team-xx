namespace exam_system.Features.Shared.ResultPattern;
public sealed record Error(string Code, string Description, ErrorType Type)
{
    /* Fields */
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.NoError);

    /* Methods */
    public static Error Validation(string code, string description)
        => new(code, description, ErrorType.Validation);

    public static Error NotFound(string code, string description)
        => new(code, description, ErrorType.NotFound);

    public static Error Unauthorized(string code, string description)
        => new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description)
        => new(code, description, ErrorType.Forbidden);

    public static Error Conflict(string code, string description)
        => new(code, description, ErrorType.Conflict);

    public static Error InternalServerError(string code, string description)
        => new(code, description, ErrorType.InternalServerError);
}
namespace exam_system.Features.Shared.ResultPattern;
public enum ErrorType
{
    NoError = 0,
    Validation = 1,
    NotFound = 2,
    Unauthorized = 3,
    Forbidden = 4,
    Conflict = 5,
    InternalServerError = 6
}

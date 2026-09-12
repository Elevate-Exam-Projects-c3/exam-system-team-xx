namespace exam_system.Shared
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = Error.Validation("Validation", "One or more validation errors occurred.");

        Error[] Errors { get; }
    }
}

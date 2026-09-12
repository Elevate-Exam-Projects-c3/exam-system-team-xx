namespace exam_system.Shared
{
    public class ValidationResult<TValue> : Result<TValue>, IValidationResult
    {
        private ValidationResult(Error[] errors) : base(errors)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        public static ValidationResult<TValue> WithErrors(Error[] errors) => new ValidationResult<TValue>(errors);
    }
}

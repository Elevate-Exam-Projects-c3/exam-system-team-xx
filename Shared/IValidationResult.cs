namespace exam_system.Shared
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = new (
                code: "ValidationError" , 
                message: "One or more validation errors occurred."
            );

        Error[] Errors { get; }
    }
}

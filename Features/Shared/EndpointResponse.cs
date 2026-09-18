using exam_system.Shared;

namespace exam_system.Features.Shared;

public class EndpointResponse<T> : ApiResponse<T>
{
    public EndpointResponse() { }

    public EndpointResponse(bool success, int statusCode, string message, T? data = default, IDictionary<string, string[]>? errors = null)
        : base(success, statusCode, message, data, errors)
    {
    }

    public static EndpointResponse<T> FromResult(RequestResponse<T> result)
    {
        return new EndpointResponse<T>(
            result.Success,
            result.StatusCode,
            result.Message,
            result.Data,
            result.Errors
        );
    }

    public static EndpointResponse<T> FromResult(Result<T> result, string successMessage = "Success", int successStatusCode = 200, int failureStatusCode = 400)
    {
        return new EndpointResponse<T>(
            result.IsSuccess,
            result.IsSuccess ? successStatusCode : failureStatusCode,
            result.IsSuccess ? successMessage : result.Error.Message,
            result.IsSuccess ? result.Value : default,
            result.IsSuccess
                ? null
                : new Dictionary<string, string[]>
                {
                    { result.Error.Code ?? "Error", new[] { result.Error.Message } }
                }
        );
    }
}

public class EndpointResponse : ApiResponse
{
    public static EndpointResponse FromResult(RequestResponse result)
    {
        return new EndpointResponse
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Message = result.Message,
            Errors = result.Errors,
            Timestamp = DateTime.UtcNow
        };
    }
}

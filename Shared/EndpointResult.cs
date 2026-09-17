using Azure;
using exam_system.Features.Shared;

namespace exam_system.Shared
{
    public class EndpointResult<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public static EndpointResponse<T> FromRequestResponse(RequestResponse<T> response)
        {
            return new EndpointResponse<T>
            {
                StatusCode = response.Success ? 200 : 400,
                Message = response.Message,
                Data = response.Data
            };

        }
    }
}

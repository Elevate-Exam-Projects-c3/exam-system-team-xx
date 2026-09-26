namespace exam_system.Common.Middleware;

using Microsoft.AspNetCore.Diagnostics;

internal sealed class GlobalExceptionHandlerMiddleware : IExceptionHandler
{

    /* Fields */
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    /* Constructor */
    public GlobalExceptionHandlerMiddleware(IProblemDetailsService problemDetailsService
        , ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    /* Methods */
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Log the thrown unhandled exception
        _logger.LogError(exception, "Unhandled Exception : {ExceptionMessage}", exception.Message);

        // Set the Http Response status code to 500: Internal Server Error
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // Compose ProblemDetailContext object to be sent on response
        var problemDetailsContext = new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new()
            {
                Type = exception.GetType().Name,
                Title = "Interal Server Error",
                Detail = exception.Message
            }
        };

        // Write the ProblemDetails object to current context response via _problemDetailsService
        return await _problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
using exam_system.Application.Common;
using exam_system.Features.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Shared;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private ActionResult<ApiResponse> Success(string? successMessage = null, int successCode = StatusCodes.Status200OK)
        => successCode switch
        {
            StatusCodes.Status200OK => Ok(ApiResponse.Ok(successMessage, HttpContext.TraceIdentifier)),
            StatusCodes.Status201Created => Created(default(string),ApiResponse.Ok(successMessage, HttpContext.TraceIdentifier)),
            _ => Ok(ApiResponse.Ok(successMessage, HttpContext.TraceIdentifier)),
        };

    private ActionResult<ApiResponse<TData>> Success<TData>(TData data, string? successMessage = null, PaginationMeta? pagination = null, int successCode = StatusCodes.Status200OK)
        => successCode switch
        {
            StatusCodes.Status200OK => Ok(ApiResponse<TData>.Ok(data, successMessage, HttpContext.TraceIdentifier, pagination)),
            StatusCodes.Status201Created => Created(default(string),ApiResponse<TData>.Ok(data, successMessage, HttpContext.TraceIdentifier, pagination)),
            _ => Ok(ApiResponse<TData>.Ok(data, successMessage, HttpContext.TraceIdentifier, pagination)),
        };

    private ActionResult Problem(Result result)
    {
        var type = $"https://www.example.com/errors/{result.Errors[0]!.Code.ToLower()}";

        var statusCode = result.Errors.FirstOrDefault()!.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = result.Errors.FirstOrDefault()!.Type switch
        {
            ErrorType.Validation => "Validation Failed",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Access Denied",
            ErrorType.Conflict => "Conflict",
            _ => "Internal Server Error"
        };

        var problems = new Dictionary<string, object?>()
        {
            ["type"] = type,
            ["title"] = title,
            ["status"] = statusCode
        };

        if(statusCode is StatusCodes.Status400BadRequest)
        {
            var errors = new Dictionary<string, string[]>();

            errors[result.Errors[0].Code] = [..result.Errors.Select(e => e.Description)];
            problems["errors"] = errors; 
        }
        else
        { 
            problems["message"] = result.Errors[0]!.Description; 
        }
        
        problems["traceId"] = HttpContext.TraceIdentifier;

        return new ObjectResult(problems)
        {
            StatusCode = statusCode
        };
    }

    protected ActionResult<ApiResponse> FromResult (Result result, string? successMessage = null, int successCode = StatusCodes.Status200OK)
    {
        var code = successCode switch
        {
            StatusCodes.Status200OK => StatusCodes.Status200OK,
            StatusCodes.Status201Created => StatusCodes.Status201Created,
            _ => StatusCodes.Status200OK
        };

        return result.IsSuccessful?
            Success(successMessage, successCode: code) :
            Problem(result);
    }

    protected ActionResult<ApiResponse<TData>> FromResult<TData>(Result<TData> dataResult, string? successMessage = null, int successCode = StatusCodes.Status200OK)
    {
        var code = successCode switch
        {
            StatusCodes.Status200OK => StatusCodes.Status200OK,
            StatusCodes.Status201Created => StatusCodes.Status201Created,
            _ => StatusCodes.Status200OK
        };

        return dataResult.IsSuccessful?
            Success(dataResult.Value, successMessage, successCode: code) :
            Problem(dataResult);
    }

    protected ActionResult<ApiResponse<IReadOnlyList<TData>>> FromResultPaginated<TData>
    (
        Result<PaginatedResult<TData>> dataResult,
        int? pageNumber,
        int? pageSize,
        string? successMessage = null
    )
    {
        if (dataResult.IsSuccessful)
            return pageNumber.HasValue && pageSize.HasValue ?
                Success(dataResult.Value.Items, successMessage, new PaginationMeta
                (
                    pageNumber.Value,
                    pageSize.Value,
                    dataResult.Value.TotalCount
                )) :
                Success(dataResult.Value.Items, successMessage);

        return Problem(dataResult);
    } 
}

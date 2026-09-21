using exam_system.Application.Common;
using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Features.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiplomaController : BaseController
    {
        public DiplomaController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Gets a paginated list of diplomas.
        /// Returns only diplomas that currently have at least one published quiz.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("GetDiplomas")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<GetDiplomasDTO>>>> GetDiplomas([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetDiplomasQuery(pageIndex, pageSize));

            if(result.Items.Count == 0)
            {
                return new ApiResponse<IReadOnlyList<GetDiplomasDTO>>();
            }

            return FromResultPaginated(
                Result<PaginatedResult<GetDiplomasDTO>>.Success(result),
                pageIndex,
                pageSize,
                "Diplomas retrieved successfully.");
        }
    }
}

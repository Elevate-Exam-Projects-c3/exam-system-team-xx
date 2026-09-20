using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a paginated list of diplomas.
        /// Returns only diplomas that currently have at least one published quiz.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("GetDiplomas")]
        public async Task<EndpointResponse<PaginatedResult<GetDiplomasDTO>>> GetDiplomas([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetDiplomasQuery(pageIndex, pageSize));
            return EndpointResponse<PaginatedResult<GetDiplomasDTO>>.FromResult(result, "Diplomas retrieved successfully.");
        }
    }
}

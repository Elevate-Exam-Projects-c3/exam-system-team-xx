using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Shared;
using exam_system.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries
{
    public record GetDiplomasQuery(int PageIndex = 1, int PageSize = 10) : IRequest<Result<PaginatedResult<GetDiplomasDTO>>>;
}

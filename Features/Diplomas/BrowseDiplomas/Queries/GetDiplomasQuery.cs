
using exam_system.Application.Common;
using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries
{
    public record GetDiplomasQuery(int PageIndex = 1, int PageSize = 10) : IRequest<PaginatedResult<GetDiplomasDTO>>;
}

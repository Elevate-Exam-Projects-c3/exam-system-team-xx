using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries
{
    public record GetDiplomasQuery():IRequest<IEnumerable<GetDiplomasDTO>>;
}

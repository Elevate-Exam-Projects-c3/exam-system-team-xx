using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers
{
    public class GetDiplomasHandle : IRequestHandler<GetDiplomasQuery, IEnumerable<GetDiplomasDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDiplomasHandle(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<GetDiplomasDTO>> Handle(GetDiplomasQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Diplomas.BrowseDiplomas.Specifications;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using exam_system.Shared;
using exam_system.Specifications;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers
{
    public class GetDiplomasHandler : IRequestHandler<GetDiplomasQuery, Result<PaginatedResult<GetDiplomasDTO>>>
    {
        private readonly IGenericRepository<Diploma> _repository;

        public GetDiplomasHandler(IGenericRepository<Diploma> repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginatedResult<GetDiplomasDTO>>> Handle(GetDiplomasQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var specification = new GetDiplomasSpecification(pageIndex, pageSize);

            var query = SpecificationEvaluator<Diploma>.GetQuery(_repository.GetAll(), specification);

            var totalCount = await _repository.CountAsync(specification.Criteria);
            var diplomas = await query.ToListAsync(cancellationToken);

            TypeAdapterConfig<Diploma, GetDiplomasDTO>.NewConfig()
                .Map(dest => dest.QuizzesCount, src => src.Quizzes.Count());

            var diplomasDto = diplomas.Adapt<List<GetDiplomasDTO>>();
            var paginatedResult = PaginatedResult<GetDiplomasDTO>.Create(diplomasDto, totalCount, pageIndex, pageSize);

            return Result.Success(paginatedResult);
        }
    }
}

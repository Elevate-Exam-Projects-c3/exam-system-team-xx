using exam_system.Application.Common;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Persistence.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers
{
    public class GetDiplomasHandler : IRequestHandler<GetDiplomasQuery, PaginatedResult<GetDiplomasDTO>>
    {
        private readonly IGenericRepository<Diploma> _repository;

        public GetDiplomasHandler(IGenericRepository<Diploma> repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetDiplomasDTO>> Handle(GetDiplomasQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;


            var query = _repository.GetAll()
                .Where(d => d.Quizzes.Any(q => q.Status == QuizStatus.Published))
                .Include(d => d.Quizzes.Where(q => q.Status == QuizStatus.Published))
                .Include(d => d.Enrollments);

            var totalCount = await query.CountAsync(cancellationToken);

            var diplomas = await query
                .OrderBy(d => d.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            #region Profile configuration
            // Profile configuration for mapping Diploma to GetDiplomasDTO
            TypeAdapterConfig<Diploma, GetDiplomasDTO>.NewConfig()
                .Map(dest => dest.QuizzesCount, src => src.Quizzes.Count());

            var diplomasDto = diplomas.Adapt<List<GetDiplomasDTO>>();
            var paginatedResult = new PaginatedResult<GetDiplomasDTO>(diplomasDto, totalCount);
            //
            #endregion


            return paginatedResult;
        }
    }
}

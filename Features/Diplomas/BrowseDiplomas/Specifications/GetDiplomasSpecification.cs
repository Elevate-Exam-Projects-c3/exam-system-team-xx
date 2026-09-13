using exam_system.Domain.Entities.Diplomas;
using exam_system.Specifications;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Specifications
{
    public class GetDiplomasSpecification : BaseSpecification<Diploma>
    {
        public GetDiplomasSpecification(int pageIndex, int pageSize)
            : base(_ => true)
        {
            AddInclude(d => d.Quizzes);
            AddOrderBy(d => d.Title);
            applyPaging(pageSize, pageIndex);
        }
    }
}

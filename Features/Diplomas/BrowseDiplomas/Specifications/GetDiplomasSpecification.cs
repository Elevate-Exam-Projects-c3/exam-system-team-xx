using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Specifications;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Specifications
{
    public class GetDiplomasSpecification : BaseSpecification<Diploma>
    {
        public GetDiplomasSpecification(int pageIndex, int pageSize)
            : base(d => d.Quizzes.Any(q => q.Status == QuizStatus.Published))
        {
            AddInclude(d => d.Quizzes.Where(q => q.Status == QuizStatus.Published));

            AddOrderBy(d => d.Title);
            applyPaging(pageSize, pageIndex);
        }
    }
}

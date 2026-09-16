using exam_system.Domain.Entities.Diplomas;
using exam_system.Specification;

namespace exam_system;

public class AllDiplomasSpecification : Specification<Diploma,Guid, AllDiplomasDto>
{
    public AllDiplomasSpecification()
    {
        Query.Select(d => new AllDiplomasDto
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            QuizzesCount = d.Quizzes.Count,
            EnrollmentsCount = d.Enrollments.Count,
            CreatedAt = d.CreatedAtUtc
        });
    }
}

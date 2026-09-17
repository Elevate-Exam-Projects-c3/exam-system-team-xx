using exam_system.Domain.Common;
using System.Linq.Expressions;

namespace exam_system.Specifications.Contracts
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        // Include
        ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; }

        // Where
        Expression<Func<TEntity, bool>>? Criteria { get; }

        // OrderBy
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDescending { get; }

        //Pagination
        int Skip { get; }
        int Take { get; }
        bool IsPagingEnabled { get; }
    }
}
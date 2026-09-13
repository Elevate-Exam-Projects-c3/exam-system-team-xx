
using exam_system.Domain.Common;
using exam_system.Specifications.Contracts;
using System.Linq.Expressions;

namespace exam_system.Specifications
{
    public class BaseSpecification<TEntity>(Expression<Func<TEntity, bool>> criteriaExpression)
        : ISpecification<TEntity> where TEntity : BaseEntity
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        public Expression<Func<TEntity, bool>> Criteria { get; private set; } = criteriaExpression;


        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }


        #region Sort
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }


        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        #endregion


        #region Pagination
        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void applyPaging(int pageSize, int pageIndex)
        {
            IsPagingEnabled = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
        #endregion
    }
}

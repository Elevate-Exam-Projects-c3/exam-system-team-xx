using exam_system.Domain.Common;
using exam_system.Specification.Includes;
using exam_system.Specification.Orders;
using System.Linq.Expressions;

namespace exam_system.Specification;

public interface ISpecification <TEntity,TKey> 
    where TEntity : BaseEntity<TKey>
{
    IReadOnlyList<Expression<Func<TEntity, bool>>> WhereExpressions { get; }
    IReadOnlyList<Expression<Func<TEntity, object>>> IncludesExpressions { get; }
    IReadOnlyList<ThenIncludeExpressionInfo> ThenIncludeExpressions{ get; }
    IReadOnlyList<OrderExpressionInfo<TEntity>> OrderExpressions{ get; }
    int Take { get; }
    int Skip { get; }
    bool IsPaginated { get; }
}
public interface ISpecification <TEntity,TKey,TResult> : ISpecification<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    Expression<Func<TEntity,TResult>>? SelectExpression { get; }
    Expression<Func<TEntity,IEnumerable<TResult>>>? SelectManyExpression { get; }
}

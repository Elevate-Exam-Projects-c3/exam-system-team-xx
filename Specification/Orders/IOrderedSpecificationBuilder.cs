using exam_system.Domain.Common;
using System.Linq.Expressions;

namespace exam_system.Specification.Orders;

public interface IOrderedSpecificationBuilder<TEntity, TKey> : ISpecificationBuilder<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    IOrderedSpecificationBuilder<TEntity, TKey> ThenBy(Expression<Func<TEntity, object?>> orderExpression); 
    IOrderedSpecificationBuilder<TEntity, TKey> ThenByDescending(Expression<Func<TEntity, object?>> orderExpression);
}

public interface IOrderedSpecificationBuilder<TEntity, TKey, TResult> : ISpecificationBuilder<TEntity, TKey, TResult> 
    where TEntity : BaseEntity<TKey>
{
    IOrderedSpecificationBuilder<TEntity, TKey, TResult> ThenBy(Expression<Func<TEntity, object?>> orderExpression); 
    IOrderedSpecificationBuilder<TEntity, TKey, TResult> ThenByDescending(Expression<Func<TEntity, object?>> orderExpression);
}
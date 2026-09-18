using exam_system.Domain.Common;
using exam_system.Specification.Includes;
using exam_system.Specification.Orders;
using System.Linq.Expressions;

namespace exam_system.Specification;

public interface ISpecificationBuilder<TEntity,TKey>
    where TEntity : BaseEntity<TKey>
{
    ISpecificationBuilder<TEntity,TKey> Where(Expression<Func<TEntity,bool>> predicate);

    IIncludableSpecificationBuilder<TEntity,TKey,TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression);
    IIncludableCollectionSpecificationBuilder<TEntity,TKey, TElement> Include<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> includeExpression);

    IOrderedSpecificationBuilder<TEntity,TKey> OrderBy(Expression<Func<TEntity,object?>> orderExpression);
    IOrderedSpecificationBuilder<TEntity,TKey> OrderByDescending(Expression<Func<TEntity,object?>> orderExpression);

    ISpecificationBuilder<TEntity,TKey> Skip(int skipValue);
    ISpecificationBuilder<TEntity,TKey> Take(int takeValue);
}

public interface ISpecificationBuilder<TEntity,TKey,TResult>
    where TEntity : BaseEntity<TKey>
{
    ISpecificationBuilder<TEntity,TKey,TResult> Where(Expression<Func<TEntity,bool>> predicate);

    IOrderedSpecificationBuilder<TEntity,TKey,TResult> OrderBy(Expression<Func<TEntity,object?>> orderExpression);
    IOrderedSpecificationBuilder<TEntity,TKey,TResult> OrderByDescending(Expression<Func<TEntity,object?>> orderExpression);

    ISpecificationBuilder<TEntity,TKey,TResult> Skip(int skipValue);
    ISpecificationBuilder<TEntity,TKey,TResult> Take(int takeValue);

    ISpecificationBuilder<TEntity, TKey, TResult> Select(Expression<Func<TEntity, TResult>> selectExpression);
    ISpecificationBuilder<TEntity, TKey, TResult> SelectMany(Expression<Func<TEntity, IEnumerable<TResult>>> selectManyExpression);
}
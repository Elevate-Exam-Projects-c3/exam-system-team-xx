using exam_system.Domain.Common;
using exam_system.Specification.Includes;
using exam_system.Specification.Orders;
using System.Linq.Expressions;

namespace exam_system.Specification;

internal class SpecificationBuilder<TEntity,TKey> : ISpecificationBuilder<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    /* Fields */
    protected Specification<TEntity, TKey> _specification;

    /* Constructor */
    public SpecificationBuilder(Specification<TEntity, TKey> specification)
    {
        _specification = specification;
    }

    /* Methods */
    public ISpecificationBuilder<TEntity, TKey> Where(Expression<Func<TEntity, bool>> predicate)
    {
        _specification.AddWhere(predicate);
        return this;
    }

    public IIncludableSpecificationBuilder<TEntity, TKey, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
    {
        _specification.AddInclude(includeExpression);
        return new IncludableSpecificationBuilder<TEntity, TKey, TProperty>(_specification, includeExpression);
    }

    public IIncludableCollectionSpecificationBuilder<TEntity, TKey, TElement> Include<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> includeExpression)
    {
        _specification.AddInclude(includeExpression);
        return new IncludableCollectionSpecificationBuilder<TEntity, TKey, TElement>(_specification, includeExpression);
    }

    public IOrderedSpecificationBuilder<TEntity, TKey> OrderBy(Expression<Func<TEntity, object?>> orderExpression)
    {
        _specification.AddOrder(orderExpression, OrderType.OrderBy);
        return new OrderedSpecificationBuilder<TEntity, TKey>(_specification);
    }

    public IOrderedSpecificationBuilder<TEntity, TKey> OrderByDescending(Expression<Func<TEntity, object?>> orderExpression)
    {
        _specification.AddOrder(orderExpression, OrderType.OrderByDescending);
        return new OrderedSpecificationBuilder<TEntity, TKey>(_specification);
    }

    public ISpecificationBuilder<TEntity, TKey> Skip(int skipValue)
    {
        _specification.AddSkip(skipValue);
        return this;
    }

    public ISpecificationBuilder<TEntity, TKey> Take(int takeValue)
    {
        _specification.AddTake(takeValue);
        return this;
    }
}
internal class SpecificationBuilder<TEntity, TKey, TResult> : ISpecificationBuilder<TEntity, TKey, TResult>
    where TEntity : BaseEntity<TKey>
{
    /* Fields */
    private ISpecificationBuilder<TEntity, TKey> _builder;
    protected Specification<TEntity, TKey, TResult> _specification;

    /* Constructor */
    public SpecificationBuilder(Specification<TEntity, TKey, TResult> specification)
    {
        _specification = specification;
        _builder = new SpecificationBuilder<TEntity, TKey>(specification);
    }

    /* Methods */
    public ISpecificationBuilder<TEntity, TKey, TResult> Where(Expression<Func<TEntity, bool>> predicate)
    {
        _builder.Where(predicate);
        return this;
    }

    public IOrderedSpecificationBuilder<TEntity, TKey, TResult> OrderBy(Expression<Func<TEntity, object?>> orderExpression)
    {
        _builder.OrderBy(orderExpression);
        return new OrderedSpecificationBuilder<TEntity, TKey, TResult>(_specification);
    }

    public IOrderedSpecificationBuilder<TEntity, TKey, TResult> OrderByDescending(Expression<Func<TEntity, object?>> orderExpression)
    {
        _builder.OrderByDescending(orderExpression);
        return new OrderedSpecificationBuilder<TEntity, TKey, TResult>(_specification);
    }

    public ISpecificationBuilder<TEntity, TKey, TResult> Select(Expression<Func<TEntity, TResult>> selectExpression)
    {
        _specification.SetSelect(selectExpression);
        return this;
    }

    public ISpecificationBuilder<TEntity, TKey, TResult> SelectMany(Expression<Func<TEntity, IEnumerable<TResult>>> selectManyExpression)
    {
        _specification.SetSelectMany(selectManyExpression);
        return this;
    }

    public ISpecificationBuilder<TEntity, TKey, TResult> Skip(int skipValue)
    {
        _builder.Skip(skipValue);
        return this;
    }

    public ISpecificationBuilder<TEntity, TKey, TResult> Take(int takeValue)
    {
        _builder.Take(takeValue);
        return this;
    }
}
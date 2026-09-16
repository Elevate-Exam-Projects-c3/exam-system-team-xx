using exam_system.Domain.Common;
using System.Linq.Expressions;

namespace exam_system.Specification.Orders;

internal sealed class OrderedSpecificationBuilder<TEntity, TKey> : SpecificationBuilder<TEntity, TKey>, IOrderedSpecificationBuilder<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    /* Constructor */
    public OrderedSpecificationBuilder(Specification<TEntity, TKey> specification)
        : base(specification)
    {
    }

    /* Methods */
    public IOrderedSpecificationBuilder<TEntity, TKey> ThenBy(Expression<Func<TEntity, object?>> orderExpression)
    {
        _specification.AddOrder(orderExpression,OrderType.ThenBy);
        return new OrderedSpecificationBuilder<TEntity, TKey>(_specification);
    }

    public IOrderedSpecificationBuilder<TEntity, TKey> ThenByDescending(Expression<Func<TEntity, object?>> orderExpression)
    {
        _specification.AddOrder(orderExpression, OrderType.ThenByDescending);
        return new OrderedSpecificationBuilder<TEntity, TKey>(_specification);
    }
}

internal sealed class OrderedSpecificationBuilder<TEntity, TKey, TResult> : SpecificationBuilder<TEntity, TKey, TResult>, IOrderedSpecificationBuilder<TEntity, TKey, TResult>
    where TEntity : BaseEntity<TKey>
{
    /* Constructor */
    public OrderedSpecificationBuilder(Specification<TEntity, TKey, TResult> specification)
        : base(specification)
    {
    }

    /* Methods */
    public IOrderedSpecificationBuilder<TEntity, TKey, TResult> ThenBy(Expression<Func<TEntity, object?>> orderExpression)
    {
        _specification.AddOrder(orderExpression,OrderType.ThenBy);
        return new OrderedSpecificationBuilder<TEntity, TKey, TResult>(_specification);
    }

    public IOrderedSpecificationBuilder<TEntity, TKey, TResult> ThenByDescending(Expression<Func<TEntity, object?>> orderExpression)
    {
        _specification.AddOrder(orderExpression, OrderType.ThenByDescending);
        return new OrderedSpecificationBuilder<TEntity, TKey, TResult>(_specification);
    }
}

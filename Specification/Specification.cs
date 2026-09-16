using exam_system.Domain.Common;
using exam_system.Specification.Includes;
using exam_system.Specification.Orders;
using System.Linq.Expressions;

namespace exam_system.Specification;

public abstract class Specification<TEntity, TKey> : ISpecification<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    /* Fields */
    private List<Expression<Func<TEntity, bool>>> _whereExpressions = [];
    private List<Expression<Func<TEntity, object>>> _includes = [];
    private List<ThenIncludeExpressionInfo> _thenIncludeExpressions = [];
    private List<OrderExpressionInfo<TEntity>> _orderExpressions = [];

    /* Properties */
    protected ISpecificationBuilder<TEntity,TKey> Query => new SpecificationBuilder<TEntity,TKey>(this); // builder
    public IReadOnlyList<Expression<Func<TEntity, bool>>> WhereExpressions 
        => _whereExpressions;
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes 
        => _includes;
    public IReadOnlyList<ThenIncludeExpressionInfo> ThenIncludeExpressions 
        => _thenIncludeExpressions;
    public IReadOnlyList<OrderExpressionInfo<TEntity>> OrderExpressions 
        => _orderExpressions;
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPaginated => Take > 0 || Skip > 0;

    /* Mehtods */
    internal void AddWhere(Expression<Func<TEntity, bool>> predicate)
        => _whereExpressions.Add(predicate);

    internal void AddInclude<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
    {
        Expression<Func<TEntity, object>> newIncludeExpression = Expression.Lambda<Func<TEntity, object>>(
            includeExpression.Body,
            includeExpression.Parameters);

        _includes.Add(newIncludeExpression); 
    }

    internal void AddThenInclude<TProperty>(LambdaExpression navigtion, LambdaExpression parentExpression)
        => _thenIncludeExpressions.Add(new(navigtion,parentExpression));

    internal void AddOrder(Expression<Func<TEntity,object?>> keySelector, OrderType orderType)
        => _orderExpressions.Add(new(keySelector,orderType));

    internal void AddSkip(int skipValue)
        => Skip = skipValue;

    internal void AddTake(int takeValue)
        => Take = takeValue;
}

public abstract class Specification<TEntity, TKey, TResult> : Specification<TEntity, TKey>, ISpecification<TEntity, TKey, TResult>
    where TEntity : BaseEntity<TKey>
{
    /* Proprties */
    protected new ISpecificationBuilder<TEntity, TKey, TResult> Query => new SpecificationBuilder<TEntity, TKey, TResult>(this); // builder
    public Expression<Func<TEntity, TResult>>? SelectExpression { get; private set; }
    public Expression<Func<TEntity, IEnumerable<TResult>>>? SelectManyExpression { get; private set; }

    /* Methods */
    internal void SetSelect(Expression<Func<TEntity, TResult>> selectExpression)
        => SelectExpression = selectExpression;
    internal void SetSelectMany(Expression<Func<TEntity, IEnumerable<TResult>>> selectManyExpression)
        => SelectManyExpression = selectManyExpression;
}
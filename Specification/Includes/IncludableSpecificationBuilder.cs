using exam_system.Domain.Common;
using System.Linq.Expressions;

namespace exam_system.Specification.Includes;

internal sealed class IncludableSpecificationBuilder<TEntity, TKey, TProperty> : SpecificationBuilder<TEntity, TKey>, IIncludableSpecificationBuilder<TEntity, TKey, TProperty>
    where TEntity : BaseEntity<TKey>
{
    /* Fields */
    private readonly LambdaExpression _parentExpression;

    /* Constructor */
    public IncludableSpecificationBuilder(Specification<TEntity, TKey> specification, LambdaExpression parentExpression)
        :base(specification)
    {
        _parentExpression = parentExpression;
    }

    /* Methods */
    public IIncludableSpecificationBuilder<TEntity, TKey, TNext> ThenInclude<TNext>(Expression<Func<TProperty, TNext>> thenIncludeExpression)
    {
        _specification.AddThenInclude<TProperty>(thenIncludeExpression, _parentExpression);
        return new IncludableSpecificationBuilder<TEntity, TKey, TNext>(_specification, thenIncludeExpression);
    }
}

using exam_system.Domain.Common;
using System.Linq.Expressions;

namespace exam_system.Specification.Includes;

internal sealed class IncludableCollectionSpecificationBuilder<TEntity, TKey, TElement> : SpecificationBuilder<TEntity, TKey>, IIncludableCollectionSpecificationBuilder<TEntity, TKey, TElement>
    where TEntity : BaseEntity<TKey>
{
    /* Fields */
    private readonly LambdaExpression _parentExpression;

    /* Constructor */
    public IncludableCollectionSpecificationBuilder(Specification<TEntity, TKey> specification , LambdaExpression parentExpression)
        : base(specification)
    {
        _parentExpression = parentExpression;
    }

    /* Methods */
    public IIncludableSpecificationBuilder<TEntity, TKey, TNext> ThenInclude<TNext>(Expression<Func<TElement, TNext>> thenIncludeExpression)
    {
        _specification.AddThenInclude<TElement>(thenIncludeExpression, _parentExpression);
        return new IncludableSpecificationBuilder<TEntity, TKey, TNext>(_specification, thenIncludeExpression);
    }
}

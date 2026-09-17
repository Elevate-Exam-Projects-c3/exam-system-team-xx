using exam_system.Domain.Common;
using System.Linq.Expressions;

namespace exam_system.Specification.Includes;

public interface IIncludableSpecificationBuilder<TEntity, TKey, TProperty> : ISpecificationBuilder<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    IIncludableSpecificationBuilder<TEntity, TKey, TNext> ThenInclude<TNext>(Expression<Func<TProperty,TNext>> thenIncludeExpression);
}
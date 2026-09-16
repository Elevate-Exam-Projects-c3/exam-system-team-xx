using exam_system.Domain.Common;
using exam_system.Specification.Includes;
using exam_system.Specification.Orders;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exam_system.Specification;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity,TKey> (IQueryable<TEntity> query, ISpecification<TEntity, TKey> specification)
        where TEntity : BaseEntity<TKey>
    {
        if (specification.WhereExpressions.Count > 0)
        {
            query = ApplyPredictes<TEntity, TKey>(query, specification.WhereExpressions);
        }

        if (specification.Includes.Count > 0)
        {
            query = ApplyIncludes<TEntity, TKey>(query, specification.Includes, specification.ThenIncludeExpressions);
        }

        if (specification.OrderExpressions.Count > 0)
        {
            query = ApplyOrders<TEntity, TKey>(query, specification.OrderExpressions);
        }

        if(specification.IsPaginated)
        {
            query = ApplyPagination<TEntity, TKey>(query, specification.Skip, specification.Take);
        }

        return query;
    }

    public static IQueryable<TResult> GetQuery<TEntity, TKey, TResult> (IQueryable<TEntity> query, ISpecification<TEntity, TKey, TResult> specification)
        where TEntity : BaseEntity<TKey>
    {
        query = GetQuery<TEntity, TKey>(query, specification);

        return specification.SelectExpression is not null ?
            query.Select(specification.SelectExpression) :
            query.SelectMany(specification.SelectManyExpression!);
    }

    private static IQueryable<TEntity> ApplyPredictes<TEntity, TKey>(IQueryable<TEntity> query, IReadOnlyList<Expression<Func<TEntity, bool>>> whereExpressions)
        where TEntity : BaseEntity<TKey>
    {
        foreach (var whereExpression in whereExpressions) 
        {
            query = query.Where(whereExpression);
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyIncludes<TEntity, TKey>(IQueryable<TEntity> query, 
        IReadOnlyList<Expression<Func<TEntity, object>>> includes,
        IReadOnlyList<ThenIncludeExpressionInfo> thenIncludeExpressions)
        where TEntity : BaseEntity<TKey>
    {
        var memberPaths = new Dictionary<LambdaExpression, string>();

        foreach(var includeExpression in includes)
        {
            var path = GetMemberPath(includeExpression);
            query = query.Include(path);
            memberPaths[includeExpression] = path;
        }

        if(thenIncludeExpressions.Count == 0)
            return query;

        foreach(var thenIncludeExpression in thenIncludeExpressions)
        {
            if (!memberPaths.TryGetValue(thenIncludeExpression.ParentExpression, out string? parentExpressionMemberPath))
                throw new InvalidOperationException($"Couldn't find a parent expression for thenInclude expression: '{thenIncludeExpression.ThenIncludeExpression}'");

            var thenIncludeMemberPath = GetMemberPath(thenIncludeExpression.ThenIncludeExpression);
            query = query.Include($"{parentExpressionMemberPath}.{thenIncludeMemberPath}");
            memberPaths[thenIncludeExpression.ThenIncludeExpression] = thenIncludeMemberPath;
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyOrders<TEntity, TKey>(IQueryable<TEntity> query, IReadOnlyList<OrderExpressionInfo<TEntity>> orderExpressions)
        where TEntity : BaseEntity<TKey>
    {
        IOrderedQueryable<TEntity> orderedQuery = default!;

        foreach (var orderExpression in orderExpressions) 
        {
            orderedQuery = orderExpression.OrderType switch
            {
                OrderType.OrderBy => query.OrderBy(orderExpression.KeySelector),
                OrderType.OrderByDescending => query.OrderByDescending(orderExpression.KeySelector),
                OrderType.ThenBy => orderedQuery.ThenBy(orderExpression.KeySelector),
                OrderType.ThenByDescending => orderedQuery.ThenByDescending(orderExpression.KeySelector),
                _ => throw new NotSupportedException($"Unsupported order type: '{orderExpression.OrderType}'")
            };

            query = orderedQuery;
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyPagination<TEntity, TKey>(IQueryable<TEntity> query, int skipValue, int takeValue)
        where TEntity : BaseEntity<TKey>
    {
        if(skipValue > 0)
            query = query.Skip(skipValue);

        if(takeValue > 0)
            query = query.Take(takeValue);

        return query;
    }
    
    private static string GetMemberPath(LambdaExpression expression)
    {
        // Runtime check if passed expression is actually a member expression
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new NotSupportedException($"Passed expression type '{expression}' is not of type MemberExpression");
    }
}

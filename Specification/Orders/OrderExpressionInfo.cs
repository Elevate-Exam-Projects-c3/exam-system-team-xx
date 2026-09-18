using System.Linq.Expressions;

namespace exam_system.Specification.Orders;

public sealed record OrderExpressionInfo<TEntity>(
    Expression<Func<TEntity,object?>> KeySelector,
    OrderType OrderType);


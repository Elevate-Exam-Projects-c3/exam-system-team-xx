using System.Linq.Expressions;

namespace exam_system.Specification.Includes;

public sealed record ThenIncludeExpressionInfo(
    LambdaExpression ThenIncludeExpression,
    LambdaExpression ParentExpression);

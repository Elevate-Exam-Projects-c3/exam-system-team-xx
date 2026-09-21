namespace exam_system.Application.Common;

public sealed record PaginatedResult<TResult>(IReadOnlyList<TResult> Items, int TotalCount);

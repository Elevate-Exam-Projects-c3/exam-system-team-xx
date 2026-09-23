namespace exam_system.Features.Shared;

public sealed record PaginatedResult<TResult>(IReadOnlyList<TResult> Items, int TotalCount);

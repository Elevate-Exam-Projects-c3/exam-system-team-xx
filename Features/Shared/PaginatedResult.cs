namespace E_Commerce.Application.Common;

public sealed record PaginatedResult<TResult>(IReadOnlyList<TResult> Items, int TotalCount);

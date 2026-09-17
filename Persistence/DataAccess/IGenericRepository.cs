using exam_system.Domain.Common;
using exam_system.Specification;

namespace exam_system.Persistence.DataAccess;

public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(bool trackingEnabled = true, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity,TKey> specification, bool trackingEnabled = true, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<TEntity, TKey, TResult> specification, CancellationToken cancellationToken = default);

    ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity, TKey> specification, bool trackingEnabled = true, CancellationToken cancellationToken = default);
    Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TKey, TResult> specification, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity, TKey> specification, bool trackingEnabled = true, CancellationToken cancellationToken = default);
    Task<TResult?> SingleOrDefaultAsync<TResult>(ISpecification<TEntity, TKey, TResult> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<TEntity, TKey> specification, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(ISpecification<TEntity, TKey> specification, CancellationToken cancellationToken = default);
    
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}

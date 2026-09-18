using exam_system.Domain.Common;
using exam_system.Persistence.Context;
using exam_system.Specification;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Persistence.DataAccess;

internal sealed class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    /* Fields */
    private readonly AppDbContext _storeDbContext;
    private readonly DbSet<TEntity> _entityDbSet;

    /* Constructors */
    public GenericRepository(AppDbContext storeDbContext)
    {
        _storeDbContext = storeDbContext;
        _entityDbSet = _storeDbContext.Set<TEntity>();
    }

    /* Methods */
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool trackingEnabled = true, CancellationToken cancellationToken = default)
        => trackingEnabled ?
        await _entityDbSet.ToListAsync(cancellationToken) :
        await _entityDbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity, TKey> specification, bool trackingEnabled = true, CancellationToken cancellationToken = default)
        => trackingEnabled ?
        await SpecificationEvaluator.GetQuery(_entityDbSet, specification).ToListAsync(cancellationToken) :
        await SpecificationEvaluator.GetQuery(_entityDbSet, specification).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<TEntity, TKey, TResult> specification, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_entityDbSet, specification).ToListAsync(cancellationToken);

    public async ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await _entityDbSet.FindAsync(id, cancellationToken);

    public async Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity, TKey> specification, bool trackingEnabled = true, CancellationToken cancellationToken = default)
        => trackingEnabled ?
        await SpecificationEvaluator.GetQuery(_entityDbSet, specification).FirstOrDefaultAsync(cancellationToken) :
        await SpecificationEvaluator.GetQuery(_entityDbSet, specification).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TKey, TResult> specification, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_entityDbSet, specification).FirstOrDefaultAsync(cancellationToken);

    public async Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity, TKey> specification, bool trackingEnabled = true, CancellationToken cancellationToken = default)
        => trackingEnabled ?
        await SpecificationEvaluator.GetQuery(_entityDbSet, specification).SingleOrDefaultAsync(cancellationToken) :
        await SpecificationEvaluator.GetQuery(_entityDbSet, specification).AsNoTracking().SingleOrDefaultAsync(cancellationToken);

    public async Task<TResult?> SingleOrDefaultAsync<TResult>(ISpecification<TEntity, TKey, TResult> specification, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_entityDbSet, specification).SingleOrDefaultAsync(cancellationToken);

    public async Task<bool> AnyAsync(ISpecification<TEntity, TKey> specification, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_entityDbSet, specification).AnyAsync(cancellationToken);

    public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_entityDbSet, specification).CountAsync(cancellationToken);

    public void Add(TEntity entity)
        => _entityDbSet.Add(entity);

    public void Update(TEntity entity)
        => _entityDbSet.Update(entity);

    public void Delete(TEntity entity)
       => _entityDbSet.Remove(entity);

}

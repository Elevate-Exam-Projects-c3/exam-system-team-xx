using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Common;
using exam_system.Persistence.Context;

namespace exam_system.Persistence.DataAccess;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(id, cancellationToken);

    public IQueryable<TEntity> GetAll() 
        => _dbSet;

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate) 
        => _dbSet.Where(predicate);

    public void Add(TEntity entity)
        => _dbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) 
        => _dbSet.AddRange(entities);

    public void Update(TEntity entity) 
        => _dbSet.Update(entity);

    public void Delete(TEntity entity) 
        => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities) 
        => _dbSet.RemoveRange(entities);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null, CancellationToken cancellationToken = default) 
        => criteria is not null ? await _dbSet.CountAsync(criteria, cancellationToken) : await _dbSet.CountAsync(cancellationToken);

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? criteria = null, CancellationToken cancellationToken = default) 
        => criteria is not null ? await _dbSet.FirstOrDefaultAsync(criteria, cancellationToken) : await _dbSet.FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? criteria = null, CancellationToken cancellationToken = default) 
        => criteria is not null ? await _dbSet.AnyAsync(criteria, cancellationToken) : await _dbSet.AnyAsync(cancellationToken);
}
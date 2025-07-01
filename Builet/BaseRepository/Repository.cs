using System.Linq.Expressions;
using Builet.Database;
using Microsoft.EntityFrameworkCore;

namespace Builet.BaseRepository;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    protected readonly AppDbContext _db;

    public Repository(AppDbContext appDbContext)
    {
        _db = appDbContext;
    }

    public async Task<TEntity?> GetAsync(TKey id)
    {
        return await _db.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _db.Set<TEntity>().ToListAsync();
    }

    public async Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        IQueryable<TEntity> query = _db.Set<TEntity>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.Where(predicate).ToListAsync();
    }


    public async Task AddAsync(TEntity entity)
    {
        await _db.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _db.Set<TEntity>().AddRangeAsync(entities);
    }

    public void Remove(TEntity entity)
    {
        _db.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _db.Set<TEntity>().RemoveRange(entities);
    }
    
    public void Update(TEntity entity)
    {
       _db.Set<TEntity>().Update(entity);
    }
    
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _db.Set<TEntity>().Where(predicate).CountAsync();
    }

    public async Task<List<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        IQueryable<TEntity> query = _db.Set<TEntity>();

        if (include != null)
        {
            query = include(query);
        }

        return await query
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = _db.Set<TEntity>();

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }
}
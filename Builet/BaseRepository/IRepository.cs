﻿using System.Linq.Expressions;

namespace Builet.BaseRepository;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<TEntity?> GetAsync(TKey id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
    );

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    
    void Update(TEntity entity);
    
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
    );
    
    Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    );
}
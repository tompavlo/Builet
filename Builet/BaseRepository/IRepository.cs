using System.Linq.Expressions;

namespace Builet.BaseRepository;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<TEntity?> GetAsync(TKey id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}
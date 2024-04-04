using System.Linq.Expressions;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync<TParam>(TParam param);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> DeleteAsync<TId>(TId id);

        Task<TEntity> UpdateAsync<TId>(TId id, TEntity entity);
    }
}

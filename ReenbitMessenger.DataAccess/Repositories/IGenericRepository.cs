using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync<TParam>(TParam param);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> FilterAsync<TKey>(Func<TEntity, bool> predicate,
            string orderBy, bool ascending = true, int startAt = 0, int take = 20);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> DeleteAsync<TId>(TId id);

        Task<TEntity> UpdateAsync<TId>(TId id, TEntity entity);
    }
}

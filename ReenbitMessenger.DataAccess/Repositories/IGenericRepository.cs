using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        Task<TEntity> GetAsync(TId id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate,
            string orderBy = "", bool ascending = true, int startAt = 0, int take = 20);
        Task<IEnumerable<TEntity>> FindAsync(string searchValue,
            string orderBy = "", bool ascending = true, int startAt = 0, int take = 20);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> DeleteAsync(TId id);

        Task<TEntity> UpdateAsync(TId id, TEntity entity);
    }
}

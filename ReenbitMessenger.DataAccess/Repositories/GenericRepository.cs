using ReenbitMessenger.DataAccess.Data;
using System.Linq.Expressions;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IdentityDataContext _context;

        public GenericRepository(IdentityDataContext context)
        {
            _context = context;
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteAsync<TId>(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        
        public Task<TEntity> GetAsync<TParam>(TParam param)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync<TId>(TId id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

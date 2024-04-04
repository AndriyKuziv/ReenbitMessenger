using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDataContext _dbContext;
        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();

        private bool _disposed;

        public UnitOfWork(IdentityDataContext dbContext)
        {
            _dbContext = dbContext;
            _disposed = false;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!repos.Keys.Contains(type))
            {
                repos[type] = new GenericRepository<TEntity>(_dbContext);
            }

            return (IGenericRepository<TEntity>)repos[type];
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }
    }
}

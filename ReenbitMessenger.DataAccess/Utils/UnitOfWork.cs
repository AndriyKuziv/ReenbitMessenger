using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDataContext _dbContext;
        private readonly ChatsDataContext _chatsDataContext;
        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();
        private readonly IServiceProvider _serviceProvider;

        private bool _disposed;

        public UnitOfWork(IdentityDataContext dbContext, ChatsDataContext chatsDataContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _chatsDataContext = chatsDataContext;
            _disposed = false;
            _serviceProvider = serviceProvider;
        }

        //public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        //{
        //    var type = typeof(TEntity);
        //    if (!repos.Keys.Contains(type))
        //    {
        //        repos[type] = new GenericRepository<TEntity>(_dbContext);
        //    }

        //    return (IGenericRepository<TEntity>)repos[type];
        //}

        public TInterface GetRepository<TInterface>() where TInterface : class
        {
            var type = typeof(TInterface);
            if (!repos.Keys.Contains(type))
            {
                repos[type] = _serviceProvider.GetService(type);
            }

            return (TInterface)repos[type];
        }

        public async Task<int> SaveAsync()
        {
            await _chatsDataContext.SaveChangesAsync();
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

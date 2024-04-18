using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessengerDataContext _dbContext;
        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();
        private readonly IServiceProvider _serviceProvider;

        private bool _disposed;

        public UnitOfWork(MessengerDataContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _disposed = false;
            _serviceProvider = serviceProvider;
        }

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

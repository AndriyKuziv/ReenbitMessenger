using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.Utils
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task<int> SaveAsync();
    }
}

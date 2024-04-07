using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.Utils
{
    public interface IUnitOfWork : IDisposable
    {
        //IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        TInterface GetRepository<TInterface>() where TInterface : class;

        Task<int> SaveAsync();
    }
}

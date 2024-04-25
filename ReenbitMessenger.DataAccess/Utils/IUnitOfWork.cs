namespace ReenbitMessenger.DataAccess.Utils
{
    public interface IUnitOfWork : IDisposable
    {
        TInterface GetRepository<TInterface>() where TInterface : class;

        Task<int> SaveAsync();
    }
}

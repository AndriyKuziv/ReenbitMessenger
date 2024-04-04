using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetAsync<TParam>(TParam param);
    }
}

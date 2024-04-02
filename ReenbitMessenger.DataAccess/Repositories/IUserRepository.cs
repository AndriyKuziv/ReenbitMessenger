using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(Guid id);

        Task<User> GetByUsernameAsync(string name);

        Task<User> AddAsync(User user);

        Task<User> UpdateAsync(Guid id, User user);

        Task<User> DeleteAsync(Guid id);

        Task SaveAsync();
    }
}

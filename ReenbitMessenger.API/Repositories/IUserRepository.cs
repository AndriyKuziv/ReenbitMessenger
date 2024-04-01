using ReenbitMessenger.API.Models.Domain;

namespace ReenbitMessenger.API.Repositories
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

using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByUsernameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<User> AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateAsync(Guid id, User user)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}

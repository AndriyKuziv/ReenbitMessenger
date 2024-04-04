using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using System.Linq.Expressions;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDataContext _dbContext;
        public UserRepository(IdentityDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync<TParam>(TParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<User> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> DeleteAsync<Guid>(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateAsync<Guid>(Guid id, User entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

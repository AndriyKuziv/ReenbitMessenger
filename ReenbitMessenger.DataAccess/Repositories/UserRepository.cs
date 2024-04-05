using Microsoft.AspNetCore.Identity;
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

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> GetAsync<TParam>(TParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IdentityUser>> FindAsync(Expression<Func<IdentityUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> AddAsync(IdentityUser entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> DeleteAsync<Guid>(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> UpdateAsync<Guid>(Guid id, IdentityUser entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> AuthenticateAsync(string email, string password)
        {
            var user = await _dbContext.Users.FindAsync(email, password);

            if (user is null)
            {
                return null;
            }

            return user;
        }
    }
}

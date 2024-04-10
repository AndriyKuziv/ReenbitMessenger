using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<IdentityUser>> FilterAsync<TKey>(Func<IdentityUser, bool> predicate,
            Func<IdentityUser, TKey> orderBy, SortOrder sortOrder = SortOrder.Ascending, int startAt = 0, int take = 20)
        {
            var users = _dbContext.Users.Where(predicate);

            var sortedList = sortOrder == SortOrder.Ascending ? users.OrderBy(orderBy) :
                users.OrderByDescending(orderBy);
            return sortedList.Skip(startAt).Take(take).ToList();
        }

        public async Task<IdentityUser> GetAsync<TParam>(TParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email);
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
            IdentityUser user = null;

            if (user is null)
            {
                return null;
            }

            return user;
        }
    }
}

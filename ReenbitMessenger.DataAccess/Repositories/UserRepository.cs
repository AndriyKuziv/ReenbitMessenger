using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;

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
            string orderBy, bool ascending = true, int startAt = 0, int take = 20)
        {
            var userProp = typeof(IdentityUser).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
                StringComparison.OrdinalIgnoreCase));

            if (userProp is null)
            {
                userProp = typeof(IdentityUser).GetProperty("UserName");
            }

            var users = _dbContext.Users.AsQueryable();

            var sortedList = ascending ? users.Where(predicate).OrderBy(usr => userProp.GetValue(usr)) :
                users.Where(predicate).OrderByDescending(usr => userProp.GetValue(usr));

            if (take <= 0)
            {
                return sortedList.ToList();
            }

            return sortedList.Skip(startAt).Take(take).ToList();
        }

        public async Task<IEnumerable<IdentityUser>> FilterAsync(string containsValue,
            string orderBy, bool ascending = true, int startAt = 0, int take = 20)
        {
            PropertyInfo[] properties = typeof(IdentityUser).GetProperties();

            return _dbContext.Users.Where(usr =>
                properties.Any(prop =>
                {
                    var value = prop.GetValue(usr);
                    if (value != null && value.ToString().ToLower().Contains(containsValue.ToLower()))
                        return true;
                    return false;
                })).ToList();
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

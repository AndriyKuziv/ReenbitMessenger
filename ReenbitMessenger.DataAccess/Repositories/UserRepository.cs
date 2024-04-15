using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MessengerDataContext _dbContext;
        public UserRepository(MessengerDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return _dbContext.Users.AsEnumerable();
        }

        public async Task<IEnumerable<IdentityUser>> FilterAsync(Func<IdentityUser, bool> predicate,
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
                return sortedList.AsEnumerable();
            }

            return sortedList.Skip(startAt).Take(take);
        }

        //public async Task<IEnumerable<IdentityUser>> FilterAsync(string containsValue,
        //    string orderBy, bool ascending = true, int startAt = 0, int take = 20)
        //{
        //    PropertyInfo[] properties = typeof(IdentityUser).GetProperties();

        //    return _dbContext.Users.Where(usr =>
        //        properties.Any(prop =>
        //        {
        //            var value = prop.GetValue(usr);
        //            if (value != null && value.ToString().ToLower().Contains(containsValue.ToLower()))
        //                return true;
        //            return false;
        //        })).ToList();
        //}

        public async Task<IdentityUser> GetAsync(string id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<IdentityUser>> FindAsync(Expression<Func<IdentityUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> AddAsync(IdentityUser user)
        {
            var result = await _dbContext.Users.AddAsync(user);

            return result.Entity;
        }

        public async Task<IdentityUser> DeleteAsync(string id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
            }

            return user;
        }

        public async Task<IdentityUser> UpdateAsync(string id, IdentityUser entity)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if(user is null)
            {
                return null;
            }

            user.UserName = entity.UserName;
            user.Email = entity.Email;

            return user;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Reflection;
using System.Linq.Expressions;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<IdentityUser, string>, IUserRepository
    {
        public UserRepository(MessengerDataContext dbContext) : base(dbContext){ }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return _dbContext.Users.AsEnumerable();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async new Task<IdentityUser> UpdateAsync(string id, IdentityUser entity)
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

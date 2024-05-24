using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IUserRepository : IGenericRepository<IdentityUser, string>
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<string> UpdateUserAvatarAsync(string userId, IFormFile imageFile);
        Task<string> GetUserAvatarAsync(string userId);
    }
}

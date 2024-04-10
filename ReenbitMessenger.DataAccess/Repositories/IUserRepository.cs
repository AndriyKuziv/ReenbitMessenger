using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IUserRepository : IGenericRepository<IdentityUser>
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<IdentityUser> AuthenticateAsync(string email, string password);
    }
}

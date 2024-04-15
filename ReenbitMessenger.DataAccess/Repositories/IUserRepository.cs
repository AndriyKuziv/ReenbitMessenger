using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IUserRepository : IGenericRepository<IdentityUser, string>
    {
        Task<bool> IsEmailUniqueAsync(string email);
    }
}

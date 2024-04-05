using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public interface IUserRepository : IGenericRepository<IdentityUser>
    {
        Task<IdentityUser> GetAsync<TParam>(TParam param);

        Task<IdentityUser> AuthenticateAsync(string email, string password);
    }
}

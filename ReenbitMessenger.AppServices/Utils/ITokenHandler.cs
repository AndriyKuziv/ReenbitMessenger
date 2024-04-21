using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Utils
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(IdentityUser user);
    }
}

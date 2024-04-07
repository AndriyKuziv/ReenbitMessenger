using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUserByUsernameQuery : IQuery<IdentityUser>
    {
        public string Username { get; }

        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}

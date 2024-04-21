using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Queries.User
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

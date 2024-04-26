using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.AuthServices
{
    public class LogInQuery : IQuery<IdentityUser>
    {
        public string Username { get; }
        public string Password { get; }

        public LogInQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

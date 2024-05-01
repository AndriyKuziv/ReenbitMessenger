using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class EditUserInfoCommand : ICommand<IdentityUser>
    {
        public string UserId { get; }
        public string Username { get; }
        public string Email { get; }

        public EditUserInfoCommand(string userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}

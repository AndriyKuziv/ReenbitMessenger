namespace ReenbitMessenger.DataAccess.AppServices.Commands.User
{
    public class EditUserInfoCommand : ICommand
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

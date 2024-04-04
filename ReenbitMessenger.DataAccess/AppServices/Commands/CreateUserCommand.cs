namespace ReenbitMessenger.DataAccess.AppServices.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }

        public CreateUserCommand(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}

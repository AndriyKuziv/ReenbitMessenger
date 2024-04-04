namespace ReenbitMessenger.DataAccess.AppServices.Commands
{
    public class EditUserInfoCommand : ICommand
    {
        public Guid Id { get; }
        public string Username { get; }
        public string Email { get; }

        public EditUserInfoCommand(Guid id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }
    }
}

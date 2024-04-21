namespace ReenbitMessenger.AppServices.Commands.User
{
    public class DeleteUserCommand : ICommand
    {
        public string UserId { get; }

        public DeleteUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}

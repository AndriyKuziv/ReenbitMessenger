namespace ReenbitMessenger.AppServices.UserServices.Commands
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

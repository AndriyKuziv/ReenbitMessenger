namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class DeleteUserAvatarCommand : ICommand
    {
        public string UserId { get; }

        public DeleteUserAvatarCommand(string userId)
        {
            UserId = userId;
        }
    }
}

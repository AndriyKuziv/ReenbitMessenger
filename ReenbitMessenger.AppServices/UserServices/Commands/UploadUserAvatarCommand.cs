namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class UploadUserAvatarCommand : ICommand
    {
        public string UserId { get; }
        public Stream AvatarIcon { get; }

        public UploadUserAvatarCommand(string userId, Stream avatarIcon)
        {
            UserId = userId;
            AvatarIcon = avatarIcon;
        }
    }
}

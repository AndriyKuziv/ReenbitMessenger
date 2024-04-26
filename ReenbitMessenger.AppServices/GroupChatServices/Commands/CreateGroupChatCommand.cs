using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class CreateGroupChatCommand : ICommand<GroupChat>
    {
        public string Name { get; }
        public string UserId { get; }

        public CreateGroupChatCommand(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }
    }
}

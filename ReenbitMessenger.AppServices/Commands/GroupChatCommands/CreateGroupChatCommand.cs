using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
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

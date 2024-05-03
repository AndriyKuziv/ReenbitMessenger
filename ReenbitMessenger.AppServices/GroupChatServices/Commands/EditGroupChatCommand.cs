using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class EditGroupChatCommand : ICommand<GroupChat>
    {
        public Guid GroupChatId { get; }
        public string Name { get; }

        public EditGroupChatCommand(Guid groupChatId, string name)
        {
            GroupChatId = groupChatId;
            Name = name;
        }
    }
}

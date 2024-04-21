namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
{
    public class EditGroupChatCommand : ICommand
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

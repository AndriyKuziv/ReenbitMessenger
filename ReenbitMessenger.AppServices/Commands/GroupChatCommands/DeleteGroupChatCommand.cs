namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
{
    public class DeleteGroupChatCommand : ICommand
    {
        public Guid GroupChatId { get; }

        public DeleteGroupChatCommand(Guid groupChatId)
        {
            GroupChatId = groupChatId;
        }
    }
}

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
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

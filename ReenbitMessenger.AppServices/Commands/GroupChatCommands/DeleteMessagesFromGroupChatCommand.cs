namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
{
    public class DeleteMessagesFromGroupChatCommand : ICommand
    {
        public Guid GroupChatId { get; }
        public IEnumerable<long> MessagesIds { get; }

        public DeleteMessagesFromGroupChatCommand(Guid groupChatId, IEnumerable<long> messagesIds)
        {
            GroupChatId = groupChatId;
            MessagesIds = messagesIds;
        }
    }
}

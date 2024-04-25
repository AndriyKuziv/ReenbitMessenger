using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
{
    public class DeleteMessagesFromGroupChatCommandHandler : ICommandHandler<DeleteMessagesFromGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessagesFromGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteMessagesFromGroupChatCommand command)
        {
            foreach (var msgId in command.MessagesIds)
            {
                var message = await _unitOfWork.GetRepository<IGroupChatRepository>()
                .DeleteGroupChatMessageAsync(command.GroupChatId, msgId);

                if (message is null)
                {
                    return false;
                }
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

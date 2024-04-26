using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands
{
    public class DeleteMessageFromGroupChatCommandHandler : ICommandHandler<DeleteMessageFromGroupChatCommand, GroupChatMessage>
    {

        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessageFromGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupChatMessage> Handle(DeleteMessageFromGroupChatCommand command)
        {
            var groupChatRepository = _unitOfWork.GetRepository<IGroupChatRepository>();

            var existingMessage = await groupChatRepository.GetMessageAsync(command.MessageId);

            if (existingMessage != null && existingMessage.SenderUserId != command.UserId)
            {
                return null;
            }

            var deletedMessage = await groupChatRepository.DeleteGroupChatMessageAsync(command.GroupChatId, command.MessageId);

            if (deletedMessage is null)
            {
                return null;
            }

            await _unitOfWork.SaveAsync();

            return deletedMessage;
        }
    }
}

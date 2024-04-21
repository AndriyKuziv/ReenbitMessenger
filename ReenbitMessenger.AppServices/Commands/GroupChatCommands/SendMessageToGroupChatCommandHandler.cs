using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands
{
    public class SendMessageToGroupChatCommandHandler : ICommandHandler<SendMessageToGroupChatCommand, GroupChatMessage>
    {

        private readonly IUnitOfWork _unitOfWork;

        public SendMessageToGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupChatMessage> Handle(SendMessageToGroupChatCommand command)
        {
            var groupChatRepo = _unitOfWork.GetRepository<IGroupChatRepository>();

            var resultMessage = await groupChatRepo.CreateGroupChatMessageAsync(new GroupChatMessage
            {
                GroupChatId = command.GroupChatId,
                SenderUserId = command.UserId,
                Text = command.Text
            });

            if (resultMessage is null)
            {
                return null;
            }

            await _unitOfWork.SaveAsync();

            return await groupChatRepo.GetMessageAsync(resultMessage.Id);
        }
    }
}

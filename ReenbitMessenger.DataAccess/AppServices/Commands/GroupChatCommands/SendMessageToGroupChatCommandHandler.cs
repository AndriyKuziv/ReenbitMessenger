using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class SendMessageToGroupChatCommandHandler : ICommandHandler<SendMessageToGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public SendMessageToGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SendMessageToGroupChatCommand command)
        {
            var result = await _unitOfWork.GetRepository<IGroupChatRepository>().CreateGroupChatMessageAsync(new Models.Domain.GroupChatMessage
            {
                GroupChatId = command.GroupChatId,
                SenderUserId = command.UserId,
                Text = command.Text
            });

            if (result is null)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

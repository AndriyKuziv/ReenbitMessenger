using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class DeleteMessageFromGroupChatCommandHandler : ICommandHandler<DeleteMessageFromGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessageFromGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteMessageFromGroupChatCommand command)
        {
            var message = await _unitOfWork.GetRepository<IGroupChatRepository>().DeleteGroupChatMessageAsync(new Guid(command.GroupChatId), command.MessageId);

            if (message is null)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

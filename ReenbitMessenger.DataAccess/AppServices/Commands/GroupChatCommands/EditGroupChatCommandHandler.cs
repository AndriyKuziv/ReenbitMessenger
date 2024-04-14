using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class EditGroupChatCommandHandler : ICommandHandler<EditGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public EditGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(EditGroupChatCommand command)
        {
            var groupChat = await _unitOfWork.GetRepository<IGroupChatRepository>().UpdateAsync(command.GroupChatId, new Models.Domain.GroupChat()
            {
                Name = command.Name
            });

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class RemoveUsersFromGroupChatCommandHandler : ICommandHandler<RemoveUsersFromGroupChatCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public RemoveUsersFromGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RemoveUsersFromGroupChatCommand command)
        {
            var repo = _unitOfWork.GetRepository<IGroupChatRepository>();

            foreach (var userId in command.UsersIds)
            {
                var chatMember = await repo.RemoveUserFromGroupChatAsync(new Guid(command.GroupChatId), userId);

                if (chatMember is null)
                {
                    return false;
                }
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class AddUsersToGroupChatCommandHandler : ICommandHandler<AddUsersToGroupChatCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUsersToGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddUsersToGroupChatCommand command)
        {
            var repo = _unitOfWork.GetRepository<IGroupChatRepository>();

            foreach(var userId in command.UsersIds)
            {
                var chatMember = await repo.AddUserToGroupChatAsync(new Models.Domain.GroupChatMember { GroupChatId = command.GroupChatId, UserId = userId });

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

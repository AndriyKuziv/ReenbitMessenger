using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class AddUsersToGroupChatCommandHandler : ICommandHandler<AddUsersToGroupChatCommand, IEnumerable<GroupChatMember>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUsersToGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupChatMember>> Handle(AddUsersToGroupChatCommand command)
        {
            var repo = _unitOfWork.GetRepository<IGroupChatRepository>();
            var chatMembers = new List<GroupChatMember>();

            foreach (var userId in command.UsersIds)
            {
                var chatMember = await repo.AddUserToGroupChatAsync(new Models.Domain.GroupChatMember { GroupChatId = command.GroupChatId, UserId = userId });

                if (chatMember is null)
                {
                    return null;
                }

                chatMembers.Add(chatMember);
            }

            await _unitOfWork.SaveAsync();

            return chatMembers;
        }
    }
}

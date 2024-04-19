﻿using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class CreateGroupChatCommandHandler : ICommandHandler<CreateGroupChatCommand, GroupChat>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateGroupChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupChat> Handle(CreateGroupChatCommand command)
        {
            var gcRepo = _unitOfWork.GetRepository<IGroupChatRepository>();

            var resultGroupChat = await gcRepo.AddAsync(new Models.Domain.GroupChat
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
            });

            if (resultGroupChat is null) return null;

            var resultMember = await gcRepo.AddUserToGroupChatAsync(new GroupChatMember
            {
                GroupChatId = resultGroupChat.Id,
                UserId = command.UserId
            });

            if (resultMember is null) return null;

            await _unitOfWork.SaveAsync();

            return resultGroupChat;
        }
    }
}

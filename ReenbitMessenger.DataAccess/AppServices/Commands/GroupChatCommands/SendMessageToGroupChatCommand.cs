﻿using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class SendMessageToGroupChatCommand : ICommand<GroupChatMessage>
    {
        public Guid GroupChatId { get; }
        public string UserId { get; }
        public string Text { get; }

        public SendMessageToGroupChatCommand(Guid groupChatId, string userId, string text)
        {
            GroupChatId = groupChatId;
            UserId = userId;
            Text = text;
        }
    }
}

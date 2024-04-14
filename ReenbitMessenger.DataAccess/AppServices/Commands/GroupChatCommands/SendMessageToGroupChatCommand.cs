using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands
{
    public class SendMessageToGroupChatCommand : ICommand
    {
        public string GroupChatId { get; }
        public string UserId { get; }
        public string Text { get; }

        public SendMessageToGroupChatCommand(string groupChatId, string userId, string text)
        {
            GroupChatId = groupChatId;
            UserId = userId;
            Text = text;
        }
    }
}

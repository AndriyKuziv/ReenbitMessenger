using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries
{
    public class GetFullGroupChatQuery : IQuery<GroupChat>
    {
        public Guid ChatId { get; }

        public GetFullGroupChatQuery(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}

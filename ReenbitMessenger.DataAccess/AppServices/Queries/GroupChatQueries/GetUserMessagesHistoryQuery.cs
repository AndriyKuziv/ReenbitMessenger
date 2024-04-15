using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries
{
    public class GetUserMessagesHistoryQuery : IQuery<IEnumerable<GroupChatMessage>>
    {
        public string UserId { get; }

        public GetUserMessagesHistoryQuery(string userId)
        {
            UserId = userId;
        }
    }
}

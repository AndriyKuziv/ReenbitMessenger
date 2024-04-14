using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries
{
    public class GetUserGroupChatsQuery : IQuery<IEnumerable<GroupChat>>
    {
        public string UserId { get; }

        public GetUserGroupChatsQuery(string userId)
        {
            UserId = userId;
        }
    }
}

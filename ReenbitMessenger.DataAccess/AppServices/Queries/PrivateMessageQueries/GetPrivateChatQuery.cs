using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.PrivateMessageQueries
{
    public class GetPrivateChatQuery : IQuery<IEnumerable<PrivateMessage>>
    {
        public string FirstUserId { get; }
        public string SecondUserId { get; }

        public GetPrivateChatQuery(string firstUserId, string secondUserId)
        {
            FirstUserId = firstUserId;
            SecondUserId = secondUserId;
        }
    }
}

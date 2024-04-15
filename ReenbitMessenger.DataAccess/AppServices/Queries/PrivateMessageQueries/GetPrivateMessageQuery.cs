using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.PrivateMessageQueries
{
    public class GetPrivateMessageQuery : IQuery<PrivateMessage>
    {
        public long MessageId { get; }

        public GetPrivateMessageQuery(long messageId)
        {
            MessageId = messageId;
        }
    }
}

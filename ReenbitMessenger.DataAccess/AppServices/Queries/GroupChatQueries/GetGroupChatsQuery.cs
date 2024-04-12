using ReenbitMessenger.DataAccess.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries
{
    public class GetGroupChatsQuery : IQuery<IEnumerable<GroupChat>>
    {
        public int NumberOfChats { get; set; }
        public int Page { get; set; }
        public string ValueContains { get; }
        public bool Ascending { get; }
        public string OrderBy { get; }

        public GetGroupChatsQuery(int numberOfChats = 20, string valueContains = "", int page = 0,
    bool ascending = true, string orderBy = "Name")
        {
            OrderBy = orderBy;
            NumberOfChats = numberOfChats;
            ValueContains = valueContains;
            Page = page;
            Ascending = ascending;
        }
    }
}

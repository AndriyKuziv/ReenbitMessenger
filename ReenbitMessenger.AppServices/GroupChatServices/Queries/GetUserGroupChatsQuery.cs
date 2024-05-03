﻿using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetUserGroupChatsQuery : IQuery<IEnumerable<GroupChat>>
    {
        public string UserId { get; }
        public int NumberOfChats { get; set; }
        public int Page { get; set; }
        public string ValueContains { get; }
        public bool Ascending { get; }
        public string OrderBy { get; }

        public GetUserGroupChatsQuery(string userId, int numberOfChats = 20, string valueContains = "", int page = 0,
            bool ascending = true, string orderBy = "Name")
        {
            UserId = userId;
            OrderBy = orderBy;
            NumberOfChats = numberOfChats;
            ValueContains = valueContains;
            Page = page;
            Ascending = ascending;
        }
    }
}

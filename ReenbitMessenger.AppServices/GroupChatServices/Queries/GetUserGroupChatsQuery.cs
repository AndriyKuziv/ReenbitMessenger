﻿using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
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
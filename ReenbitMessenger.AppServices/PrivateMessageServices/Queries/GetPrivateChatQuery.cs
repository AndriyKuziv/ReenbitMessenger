﻿using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Queries
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

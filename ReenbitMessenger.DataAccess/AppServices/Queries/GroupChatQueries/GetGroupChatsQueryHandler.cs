using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.AppServices.Queries.User;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries
{
    public class GetGroupChatsQueryHandler : IQueryHandler<GetGroupChatsQuery, IEnumerable<GroupChat>>
    {
        private readonly IGroupChatRepository _groupChatRepository;

        public GetGroupChatsQueryHandler(IGroupChatRepository groupChatRepository)
        {
            _groupChatRepository = groupChatRepository;
        }

        public async Task<IEnumerable<GroupChat>> Handle(GetGroupChatsQuery query)
        {
            return await _groupChatRepository.FilterAsync<string>(
                predicate: gc => gc.Name.Contains(query.ValueContains),
                orderBy: query.OrderBy,
                ascending: query.Ascending,
                startAt: query.Page * query.NumberOfChats,
                take: query.NumberOfChats);
        }
    }
}

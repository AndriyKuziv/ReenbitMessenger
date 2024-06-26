﻿using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetGroupChatsQueryHandler : IQueryHandler<GetGroupChatsQuery, IEnumerable<GroupChat>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGroupChatsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupChat>> Handle(GetGroupChatsQuery query)
        {
            return (await _unitOfWork.GetRepository<IGroupChatRepository>()
                .FindAsync(
                    query.ValueContains,
                    orderBy: query.OrderBy,
                    ascending: query.Ascending,
                    startAt: query.Page * query.NumberOfChats,
                    take: query.NumberOfChats)).ToList();
        }
    }
}

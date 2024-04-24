﻿using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Queries.PrivateMessageQueries
{
    public class GetPrivateChatQueryHandler : IQueryHandler<GetPrivateChatQuery, IEnumerable<PrivateMessage>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPrivateChatQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PrivateMessage>> Handle(GetPrivateChatQuery query)
        {
            return (await _unitOfWork.GetRepository<IPrivateMessageRepository>()
                .GetPrivateChatAsync(query.FirstUserId, query.SecondUserId)).ToList();
        }
    }
}
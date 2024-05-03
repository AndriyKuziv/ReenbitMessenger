using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetGroupChatInfoQueryHandler : IQueryHandler<GetGroupChatInfoQuery, GroupChat>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGroupChatInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupChat> Handle(GetGroupChatInfoQuery query)
        {
            return await _unitOfWork.GetRepository<IGroupChatRepository>()
                .GetInfoAsync(query.ChatId);
        }
    }
}

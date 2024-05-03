using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetUserGroupChatsQueryHandler : IQueryHandler<GetUserGroupChatsQuery, IEnumerable<GroupChat>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserGroupChatsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupChat>> Handle(GetUserGroupChatsQuery query)
        {
            return (await _unitOfWork.GetRepository<IGroupChatRepository>()
                .GetUserChatsAsync(query.UserId)
                ).ToList();
        }
    }
}

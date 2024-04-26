using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.GroupChatServices.Queries
{
    public class GetUserMessagesHistoryQueryHandler : IQueryHandler<GetUserMessagesHistoryQuery, IEnumerable<GroupChatMessage>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserMessagesHistoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupChatMessage>> Handle(GetUserMessagesHistoryQuery query)
        {
            return (await _unitOfWork.GetRepository<IGroupChatRepository>()
                .GetMessageHistoryAsync(
                userId: query.UserId,
                valueContains: query.ValueContains,
                orderBy: query.OrderBy,
                ascending: query.Ascending,
                startAt: query.Page * query.NumberOfChats,
                take: query.NumberOfChats))
                .ToList();
        }
    }
}

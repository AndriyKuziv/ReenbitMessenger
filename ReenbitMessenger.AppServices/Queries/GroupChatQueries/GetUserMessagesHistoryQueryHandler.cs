using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
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
            return (await _unitOfWork.GetRepository<IGroupChatRepository>().GetMessageHistoryAsync(query.UserId)).ToList();
        }
    }
}

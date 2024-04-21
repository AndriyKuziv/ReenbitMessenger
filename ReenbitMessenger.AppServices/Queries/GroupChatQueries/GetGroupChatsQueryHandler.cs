using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
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
                .FilterAsync(
                    predicate: gc => gc.Name.Contains(query.ValueContains),
                    orderBy: query.OrderBy,
                    ascending: query.Ascending,
                    startAt: query.Page * query.NumberOfChats,
                    take: query.NumberOfChats)).ToList();
        }
    }
}

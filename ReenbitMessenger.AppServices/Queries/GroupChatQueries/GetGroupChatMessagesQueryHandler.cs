using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
{
    public class GetGroupChatMessagesQueryHandler : IQueryHandler<GetGroupChatMessagesQuery, IEnumerable<GroupChatMessage>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGroupChatMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupChatMessage>> Handle(GetGroupChatMessagesQuery query)
        {
            return (await _unitOfWork.GetRepository<IGroupChatRepository>()
                .FilterMessagesAsync(
                    predicate: cmem => cmem.GroupChatId == query.GroupChatId &&
                        cmem.Text.Contains(query.MessageContains),
                    ascending: query.Ascending,
                    startAt: query.NumberOfMessages * query.Page,
                    take: query.NumberOfMessages
                    ))
                .ToList();
        }
    }
}

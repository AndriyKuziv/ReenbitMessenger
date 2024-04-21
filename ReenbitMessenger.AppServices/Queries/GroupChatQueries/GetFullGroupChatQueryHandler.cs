using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Queries.GroupChatQueries
{
    public class GetFullGroupChatQueryHandler : IQueryHandler<GetFullGroupChatQuery, GroupChat>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFullGroupChatQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupChat> Handle(GetFullGroupChatQuery query)
        {
            return await _unitOfWork.GetRepository<IGroupChatRepository>()
                .GetFullAsync(query.ChatId);
        }
    }
}

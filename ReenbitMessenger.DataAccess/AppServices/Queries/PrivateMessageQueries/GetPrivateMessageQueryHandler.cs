using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.PrivateMessageQueries
{
    public class GetPrivateMessageQueryHandler : IQueryHandler<GetPrivateMessageQuery, PrivateMessage>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPrivateMessageQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivateMessage> Handle(GetPrivateMessageQuery query)
        {
            return await _unitOfWork.GetRepository<IPrivateMessageRepository>().GetAsync(query.MessageId);
        }
    }
}

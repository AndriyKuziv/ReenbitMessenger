using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.UserServices.Queries
{
    public class GetUserAvatarByIdQueryHandler : IQueryHandler<GetUserAvatarByIdQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserAvatarByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(GetUserAvatarByIdQuery query)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            return await userRepository.GetUserAvatarAsync(query.UserId);
        }
    }
}

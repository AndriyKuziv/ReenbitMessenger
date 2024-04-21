using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.AppServices.Utils;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Queries.User
{
    public class GetUserByIdQueryHandler :
        IQueryHandler<GetUserByIdQuery, IdentityUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityUser> Handle(GetUserByIdQuery query)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var user = await userRepository.GetAsync(query.Id);

            return user;
        }
    }
}

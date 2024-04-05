using AutoMapper;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUserByIdQueryHandler :
        IQueryHandler<GetUserByIdQuery, IdentityUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityUser> Handle(GetUserByIdQuery query)
        {
            var userRepository = _unitOfWork.GetRepository<IdentityUser>();

            var user = await userRepository.GetAsync(query.Id);

            return user;
        }
    }
}

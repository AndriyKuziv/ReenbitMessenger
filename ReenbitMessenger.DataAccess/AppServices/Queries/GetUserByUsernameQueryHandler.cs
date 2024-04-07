using AutoMapper;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUserByUsernameQueryHandler :
        IQueryHandler<GetUserByUsernameQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByUsernameQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<User> Handle(GetUserByUsernameQuery query)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var user = await userRepository.GetAsync(query.Username);

            if (user is null) return null;

            var userDTO = _mapper.Map<User>(user);

            return userDTO;
        }
    }
}

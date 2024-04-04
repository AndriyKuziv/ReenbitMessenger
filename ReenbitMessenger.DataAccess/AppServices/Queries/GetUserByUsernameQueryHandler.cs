using AutoMapper;
using ReenbitMessenger.Library.Models.DTO;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

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
            var userRepository = _unitOfWork.GetRepository<User>();

            var user = await userRepository.GetAsync(query.Username);

            if (user is null) return null;

            var userDTO = _mapper.Map<User>(user);

            return userDTO;
        }
    }
}

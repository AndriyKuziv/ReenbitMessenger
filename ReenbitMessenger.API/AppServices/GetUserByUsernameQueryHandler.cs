using AutoMapper;
using ReenbitMessenger.API.Models.DTO;
using ReenbitMessenger.API.Repositories;

namespace ReenbitMessenger.API.AppServices
{
    public class GetUserByUsernameQueryHandler :
        IQueryHandler<GetUserByUsernameQuery, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserByUsernameQueryHandler(IUserRepository userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> Handle(GetUserByUsernameQuery query)
        {
            var user = await _userRepository.GetByUsernameAsync(query.Username);

            if (user is null) return null;

            var userDTO = _mapper.Map<Models.DTO.User>(user);

            return userDTO;
        }
    }
}

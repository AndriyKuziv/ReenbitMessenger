using AutoMapper;
using ReenbitMessenger.Library.Models.DTO;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.AppServices
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

            var userDTO = _mapper.Map<User>(user);

            return userDTO;
        }
    }
}

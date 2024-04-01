using AutoMapper;
using ReenbitMessenger.API.Models.DTO;
using ReenbitMessenger.API.Repositories;

namespace ReenbitMessenger.API.AppServices
{
    public class GetUserByIdQueryHandler :
        IQueryHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserByIdQueryHandler(IUserRepository userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> Handle(GetUserByIdQuery query)
        {
            var user = await _userRepository.GetByIdAsync(query.Id);

            if (user is null) return null;

            var userDTO = _mapper.Map<Models.DTO.User>(user);

            return userDTO;
        }
    }
}

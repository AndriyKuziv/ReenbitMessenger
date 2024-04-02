using AutoMapper;
using ReenbitMessenger.Library.Models.DTO;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.AppServices
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

            var userDTO = _mapper.Map<User>(user);

            return userDTO;
        }
    }
}

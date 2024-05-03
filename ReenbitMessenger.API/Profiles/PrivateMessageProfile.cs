using AutoMapper;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.API.Profiles
{
    public class PrivateMessageProfile : Profile
    {
        public PrivateMessageProfile()
        {
            CreateMap<PrivateMessage, Infrastructure.Models.DTO.PrivateMessage>()
                .ReverseMap();
        }
    }
}

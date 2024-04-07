using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DataAccess.Models.Domain.User, Infrastructure.Models.DTO.User>()
                .ReverseMap();
            CreateMap<IdentityUser, Infrastructure.Models.DTO.User>()
                .ReverseMap();
        }
    }
}

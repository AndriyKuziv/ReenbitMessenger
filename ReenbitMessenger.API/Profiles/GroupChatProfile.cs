using AutoMapper;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.API.Profiles
{
    public class GroupChatProfile : Profile
    {
        public GroupChatProfile()
        {
            CreateMap<GroupChat, Infrastructure.Models.DTO.GroupChat>()
                .ReverseMap();
            CreateMap<GroupChatMember, Infrastructure.Models.DTO.GroupChatMember>()
                .ReverseMap();
            CreateMap<GroupChatMessage, Infrastructure.Models.DTO.GroupChatMessage>()
                .ReverseMap();
            CreateMap<GroupChatRole, Infrastructure.Models.DTO.GroupChatRole>()
                .ReverseMap();
        }
    }
}

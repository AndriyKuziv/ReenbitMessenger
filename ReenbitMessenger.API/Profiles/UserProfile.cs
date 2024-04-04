﻿using AutoMapper;

namespace ReenbitMessenger.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DataAccess.Models.Domain.User, Library.Models.DTO.User>()
                .ReverseMap();
        }
    }
}

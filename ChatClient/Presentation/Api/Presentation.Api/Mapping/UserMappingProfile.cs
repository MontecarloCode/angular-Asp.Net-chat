﻿using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.Resources.Users;

namespace Presentation.Api.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, AuthenticatedUserResource>()
                .ForMember(destination => destination.Token, 
                    config => config.Ignore());

            CreateMap<User, UserProfileResource>();
        }
    }
}

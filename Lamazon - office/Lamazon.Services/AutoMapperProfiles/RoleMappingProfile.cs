﻿using AutoMapper;
using Lamazon.Domain.Entities;
using Lamazon.ViewModels.Models;

namespace Lamazon.Services.AutoMapperProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            // Define your mappings here
            CreateMap<Role, RoleViewModel>().ReverseMap();

        }
    }
}

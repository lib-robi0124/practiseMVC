using AutoMapper;
using Lamazon.Domain.Entities;
using Lamazon.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamazon.Services.AutoMapperProfiles
{   //mapping profile for Domain.User and ViewModel.UserViewModel, RegisterUserViewModel
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
         CreateMap<User, UserViewModel>().ReverseMap();
         CreateMap<User, RegisterUserViewModel>().ReverseMap();
        }
    }
}

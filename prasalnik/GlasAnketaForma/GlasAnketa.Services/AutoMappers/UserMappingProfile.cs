using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserVM>().ReverseMap();
            CreateMap<User, RegisterUserVM>().ReverseMap();
        }
    }
}

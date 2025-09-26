using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.AutoMapperProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Create your mappings here
            // Example:
            // CreateMap<Source, Destination>();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, RegisterUserViewModel>().ReverseMap();
        }
    }
}

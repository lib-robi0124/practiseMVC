using AutoMapper;
using Prasalnik.Domain.Enums;
using Prasalnik.Domain.Models;
using Prasalnik.Mapers;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Domain -> ViewModel
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            // ViewModel -> Domain
              CreateMap<UserViewModel, User>()
                .ForMember(d => d.Role, opt => opt.MapFrom<RoleResolver>());

            // RegisterUserViewModel -> User
            CreateMap<RegisterUserViewModel, User>()
                .ForMember(d => d.Role, opt => opt.MapFrom((src, dest, destMember, context) =>
                    Enum.TryParse<RoleEnum>(src.Role, out var role) ? role : RoleEnum.Employee));

            // User -> RegisterUserViewModel
            CreateMap<User, RegisterUserViewModel>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            // Credentials mapping
            CreateMap<UserCredentialsViewModel, User>()
                .ForMember(d => d.Role, opt => opt.Ignore()); // credentials don't provide role
        }
    }
}

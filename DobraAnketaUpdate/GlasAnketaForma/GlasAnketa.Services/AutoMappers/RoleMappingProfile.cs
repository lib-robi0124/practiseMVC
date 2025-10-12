using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile() { CreateMap<Role, RoleVM>().ReverseMap(); }
    }
}

using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class StatusMappingProfile : Profile
    {
        public StatusMappingProfile()
        {
            CreateMap<Status, StatusViewModel>().ReverseMap();
        }
    }
}

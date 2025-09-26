using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class AnswerMappingProfile : Profile
    {
        public AnswerMappingProfile()
        {
            CreateMap<Answer, AnswerViewModel>().ReverseMap();
        }
    }
}

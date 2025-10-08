using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class QuestionMappingProfile : Profile
    {
        public QuestionMappingProfile() 
        {
            CreateMap<Question, QuestionVM>()
            .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType.Name))
            .ReverseMap();
            CreateMap<Question, RegisterQuestionVM>().ReverseMap();
        }
    }
}

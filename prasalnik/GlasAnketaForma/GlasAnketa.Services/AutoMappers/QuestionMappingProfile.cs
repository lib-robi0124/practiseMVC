using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class QuestionMappingProfile : Profile
    {
        public QuestionMappingProfile() 
        {
            CreateMap<Question, QuestionVM >().ReverseMap();
            CreateMap<Question, RegisterQuestionVM>().ReverseMap();
        }
    }
}

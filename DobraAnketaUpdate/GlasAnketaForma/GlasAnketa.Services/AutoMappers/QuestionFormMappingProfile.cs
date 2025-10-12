using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class QuestionFormMappingProfile : Profile
    {
         public QuestionFormMappingProfile() 
        {
            CreateMap<QuestionForm, QuestionFormVM>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count))
            .ReverseMap();
            CreateMap<QuestionForm, FormSubmissionVM>().ReverseMap();
        }
    }
}

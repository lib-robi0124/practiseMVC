using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class QuestionFormMappingProfile : Profile
    {
        public QuestionFormMappingProfile() { CreateMap<QuestionForm, QuestionFormVM>().ReverseMap(); }
    }
}

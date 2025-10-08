using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class QuestionTypeMappingProfile : Profile
    {
        public QuestionTypeMappingProfile()
        {
            CreateMap<QuestionType, QuestionTypeVM>().ReverseMap();
        }
    }
}

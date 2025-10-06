using AutoMapper;
using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.AutoMappers
{
    public class AnswerMappingProfile : Profile
    {
        public AnswerMappingProfile() { CreateMap<Answer, AnswerVM>().ReverseMap(); }
    }
}

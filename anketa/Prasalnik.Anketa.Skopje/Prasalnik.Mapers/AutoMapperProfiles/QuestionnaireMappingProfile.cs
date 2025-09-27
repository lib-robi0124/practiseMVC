using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class QuestionnaireMappingProfile : Profile
    {
        public QuestionnaireMappingProfile()
        {
            // Domain -> ViewModel (including child items)
            CreateMap<Questionnaire, QuestionnaireViewModel>()
                .ForMember(d => d.QuestionItems, opt => opt.MapFrom(s => s.QuestionItems));

            // ViewModel -> Domain
            CreateMap<QuestionnaireViewModel, Questionnaire>()
                .ForMember(d => d.QuestionItems, opt => opt.MapFrom(s => s.QuestionItems));
        }
    }
}

using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.AutoMapperProfiles
{
    public class QuestionnaireMappingProfile : Profile
    {
        public QuestionnaireMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Questionnaire, QuestionnaireViewModel>();
        }
    }
}

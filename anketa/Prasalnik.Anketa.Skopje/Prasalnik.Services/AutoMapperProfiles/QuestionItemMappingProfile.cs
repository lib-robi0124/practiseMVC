using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.AutoMapperProfiles
{
    public class QuestionItemMappingProfile : Profile
    {
        public QuestionItemMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<QuestionItem, QuestionItemViewModel>();
        }
    }
}

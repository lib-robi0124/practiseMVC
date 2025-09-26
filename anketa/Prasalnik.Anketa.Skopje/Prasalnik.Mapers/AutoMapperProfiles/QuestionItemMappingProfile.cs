using AutoMapper;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;
using Prasalnik.Domain.Enums;
using System;

namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class QuestionItemMappingProfile : Profile
    {
        public QuestionItemMappingProfile()
        {
            // Domain -> ViewModel: convert enum to int
            CreateMap<QuestionItem, QuestionItemViewModel>()
                .ForMember(d => d.QuestionType, opt => opt.MapFrom(s => (int)s.Type))
                .ForMember(d => d.QuestionnaireId, opt => opt.MapFrom(s => s.QuestionnaireId));

            // ViewModel -> Domain: convert int back to enum safely
            CreateMap<QuestionItemViewModel, QuestionItem>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s =>
                    Enum.IsDefined(typeof(QuestionTypeEnum), s.QuestionType)
                        ? (QuestionTypeEnum)s.QuestionType
                        : QuestionTypeEnum.Text))
                .ForMember(d => d.QuestionnaireId, opt => opt.MapFrom(s => s.QuestionnaireId));
        }
    }
}
